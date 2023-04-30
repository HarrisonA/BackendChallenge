using BackendChallenge.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendChallenge.Models;
using BackendChallenge.Enums;
using BackendChallenge.Utils;





namespace BackendChallenge.Controllers;

[ApiController]
//  /incentives endpoint
[Route("[controller]")]
public class IncentivesController : ControllerBase
{
    private readonly ILogger<IncentivesController> _logger;
    private readonly AppDbContext _db;

    public IncentivesController(ILogger<IncentivesController> logger, AppDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    [HttpGet(Name = "GetIncentives")]
    public async Task<ActionResult> Index(CancellationToken token)
    {
        var incentivesFound = new List<Incentive>();
        try
        {   
            var userToken = Request.Headers["UserToken"].FirstOrDefault();
            if (string.IsNullOrEmpty(userToken)) {
                return BadRequest("UserToken header is missing.");
            }
            var userId = await TokenUtils.GetUserIdFromToken(userToken, token, _db);

            if (userId == null){
                return StatusCode(401, "Invalid or expired authentication token.");
            }

            // Find completed learning plans for this user
            // NOTE: Plan must be completed to be elible for incentives.
            var learningPlans = await _db.LearningPlans
            .Include(lp => lp.LearningPlanItems)
            .Where(lp => lp.UserId == userId && lp.PlanStatus == PlanStatus.Completed)
            .ToListAsync(token);

            if (learningPlans.Count == 0) {
                // TODO: Update this to return JSON object like:
                // { userId = userId, incentives = [] };
                return NotFound("No COMPLETED learning plan found for this user, thus no incentives.");
            }

            foreach (var plan in learningPlans)
            {
                var planItems = await _db.LearningPlanItems
                .Where(lpi => lpi.LearningPlanId == plan.LearningPlanId && lpi.LearningItemStatus == LearningItemStatus.Active)
                .ToListAsync(token);

                foreach (var item in planItems)
                {
                    if (item.LearningItemType != LearningItemType.Incentive)
                    {
                        continue;
                    }

                    var userCompanyId = await UserUtils.GetCompanyIdFromUserId((int)userId, token, _db);
                    if (userCompanyId == null ){
                        throw new InvalidOperationException("Unable to get CompanyId from the userID!");
                    }

                    var incentive = await FindEligibleIncentive((int) userCompanyId, (int)item.IncentiveId, token);
                    if (incentive == null)
                    {
                        continue;
                    }

                    var roleEligibility = incentive.RoleEligibility;

                    // If the user is a manager/managee, assign the managementRealtionships instance to a 
                    // appropriate variable (to be used for comparison later)
                    var manager = await _db.ManagementRelationships.FirstOrDefaultAsync(m => m.ManagerId == userId, token);
                    var managee = await _db.ManagementRelationships.FirstOrDefaultAsync(m => m.ManageeId == userId, token);

                    if (manager == null && managee == null){
                      throw new InvalidOperationException("UserId is neither a manager or managee! Must be at least one of the two");
                  }

                  if (IsRoleEligible(roleEligibility, manager, managee))
                  {
                    var userTenureDays = await _db.Users
                    .Where(u => u.UserId == userId)
                    .Select(u => u.TenureDays)
                    .SingleOrDefaultAsync(token);

                    if (userTenureDays >= incentive.ServiceRequirementDays)
                    {
                        incentivesFound.Add(incentive);
                    }
                }
            }
        }

        // TODO: IF NECESSARY, remove unneeded properties from incentives objects before returning 
        // the response. See assignment for details.
        var result = new { userId = userId, incentives = incentivesFound };
        return Ok(result);
    }
    catch (InvalidOperationException ex)
    {
        _logger.LogError(ex, "Error getting incentives");
        return StatusCode(500, "Internal server error.");
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Unexpected Error getting incentives");
        return StatusCode(500, "Internal server error.");
    }
}

// Utility method to find eligible incentives
private async Task<Incentive> FindEligibleIncentive(int companyId, int incentiveId, CancellationToken token)
{
    return await _db.Incentives
    .Where(i => i.IncentiveId == incentiveId && i.CompanyId == companyId)
    .SingleOrDefaultAsync(token);
}

// Utility method to check if the user's role is eligible for the incentive
private bool IsRoleEligible(RoleEligibility roleEligibility, ManagementRelationship manager, ManagementRelationship managee)
{
    return roleEligibility == RoleEligibility.All ||
    (manager != null && roleEligibility == RoleEligibility.Manager) ||
    (managee != null && roleEligibility == RoleEligibility.IndividualContributor);
}

}