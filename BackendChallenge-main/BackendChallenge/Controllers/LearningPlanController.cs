using BackendChallenge.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendChallenge.Models;
using BackendChallenge.Enums;
using BackendChallenge.Utils;

namespace BackendChallenge.Controllers;

[ApiController]

//  /learning-plan endpoint
[Route("learning-plan")]
public class LearningPlanController : ControllerBase
{
    private readonly ILogger<LearningPlanController> _logger;
    private readonly AppDbContext _db;

    public LearningPlanController(ILogger<LearningPlanController> logger, AppDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    [HttpGet(Name = "GetLearningPlan")]
    public async Task<ActionResult<IEnumerable<LearningPlan>>> Index(CancellationToken token)
    {
        try
        {   
            var userToken = Request.Headers["UserToken"].FirstOrDefault();
            if (string.IsNullOrEmpty(userToken)){
                return BadRequest("UserToken header is missing.");
            }

            var userId = await TokenUtils.GetUserIdFromToken(userToken, token, _db);

            if (userId == null){
                return StatusCode(401, "Invalid or expired authentication token.");
            }

            // Get the active learning plan for the current userId
            var learningPlan = await _db.LearningPlans
            .Include(lp => lp.LearningPlanItems)
            .SingleOrDefaultAsync(lp => lp.UserId == userId && lp.PlanStatus == PlanStatus.Active, token);
            

            if (learningPlan == null) {
                return NotFound("No ACTIVE learning plan found for this user.");
            }

            // Update the learning planItems with values for `LearningItemName` and `ItemId`
            // Note: Since the `learningPlan` variable is readonly, we must create a copy of the
            // planItems list and make the updates to it. The updated planItems will replace the old ones.
            var updatedPlanItems = await _db.LearningPlanItems
            .Where(lpi => lpi.LearningPlanId == learningPlan.LearningPlanId && lpi.LearningItemStatus == LearningItemStatus.Active)
            .Select(lpi => new LearningPlanItem { 
                LearningPlanItemId = lpi.LearningPlanItemId,
                LearningItemType = lpi.LearningItemType,
                LearningItemName = lpi.LearningItemName,
                ItemId = lpi.ItemId,
                IncentiveId = lpi.IncentiveId,
                CourseId = lpi.CourseId
                })
            .ToListAsync(token);


            if (updatedPlanItems.Count == 0 ){
                // TODO: Update this to return JSON object like:
                // { userId = userId, planItems=learningPlans };
                return NotFound("No ACTIVE learning plan items found for this user.");
            }

            // Loop through the updatedPlanItems
            // and add the update each one with the appropriate:
            // 1. Incentive or Course name
            // 2. itemID
            foreach (var planItem in updatedPlanItems)
            {
                var updatedPlanItem = await _db.LearningPlanItems.FirstOrDefaultAsync(lpi => lpi.LearningPlanItemId == planItem.LearningPlanItemId);                
                switch (planItem.LearningItemType)
                {
                    case LearningItemType.Incentive:
                    var incentive = await _db.Incentives
                    .Where(i => i.IncentiveId == planItem.IncentiveId)
                    .Select(i => new { i.IncentiveName, i.IncentiveId })
                    .FirstOrDefaultAsync(token);

                    // TODO: add better error handling
                    updatedPlanItem.LearningItemName = incentive?.IncentiveName ?? "Not Found.";
                    updatedPlanItem.ItemId = incentive?.IncentiveId ?? throw new Exception("Error: Incentive does not have an ID");
                    break;

                    case LearningItemType.Course:
                    var course = await _db.Courses
                    .Where(c => c.CourseId == planItem.CourseId)
                    .Select(c => new { c.CourseName, c.CourseId })
                    .FirstOrDefaultAsync(token);

                    // TODO: add better error handling
                    updatedPlanItem.LearningItemName = course?.CourseName ?? "Not Found.";
                    updatedPlanItem.ItemId = course?.CourseId ?? throw new Exception("Error: Course does not have an ID");
                    break;

                    default:
                    throw new Exception("Unknown Learning Type");
                }

                await _db.SaveChangesAsync(token);
            }

            learningPlan.LearningPlanItems = updatedPlanItems;
            
            // TODO: IF NECESSARY, remove unneeded properties from planItem objects before returning 
            // the response. See assignment for details.
            return Ok(learningPlan);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting Learning Plan");
            return StatusCode(500, "Internal server error.");
        }
    }

}

