# BackendChallenge
BackendChallenge


## How to run the code:
1. From the terminal:  dotnet run
2. Make GET request using Postman and include UserToken as a key in the request headers.  
*Example:     
key: UserToken  
value: 1DeyjK5vvSwjc9o9jYArVo2yov2SnjnXEE*   
3. Endpoints:   
/users   
/learning-plan  
/incentives  


## Questions to turn in:

 1. How many active users are there for UserToken = 1MEYQDDgwrTkYPtu7Vhfyjp7qkuGnf4ztR company?  
  Answer: 4



  2. What does the JSON of the user plan for UserToken = 1F7Xg1CJdffsnv9uEXj6GhLERQSam4xwx6 look like?  
  Answer: 
  ```
  {
    "learningPlanId": 3,
    "userId": 2,
    "user": null,
    "planStatus": 0,
    "learningPlanItems": [
        {
            "learningPlanItemId": 3,
            "learningPlanId": 0,
            "learningPlan": null,
            "learningItemType": 0,
            "learningItemStatus": 0,
            "learningItemName": "Managerial Economics",
            "itemId": 4,
            "courseId": 4,
            "course": null,
            "incentiveId": null,
            "incentive": null
        },
        {
            "learningPlanItemId": 4,
            "learningPlanId": 0,
            "learningPlan": null,
            "learningItemType": 0,
            "learningItemStatus": 0,
            "learningItemName": "Design Thinking",
            "itemId": 5,
            "courseId": 5,
            "course": null,
            "incentiveId": null,
            "incentive": null
        },
        {
            "learningPlanItemId": 5,
            "learningPlanId": 0,
            "learningPlan": null,
            "learningItemType": 1,
            "learningItemStatus": 0,
            "learningItemName": "Completion Bonus",
            "itemId": 1,
            "courseId": null,
            "course": null,
            "incentiveId": 1,
            "incentive": null
        }
    ]
}
```

3. How many incentives is UserToken = 1DeyjK5vvSwjc9o9jYArVo2yov2SnjnXEE eligible for?   
 Answer: 0 (Reason: That user does not have, incentives associated with their plan).
 
 
# Test plan
# Unit Tests

## Utility functions
- Validate that incorrect argument types generates an error
- Test that valid arguments (types and values) generates a valid response
- (if needed) add more test details that are specific to each function

## Controllers functions (Users, LearningPlan and Incentives controllers)
(Do the following for all functions created by the developer (me), i.e. not the built-in native functions)
1. Validate that incorrect argument types generates an error
2. Test that valid arguments (types and values) generates a valid response
3. Test that all function errors are caught by each controllers try/catch blocks, and the expected exception is captured
4. (if needed) Add more test details that are specific to each function

# API tests

## GET requests to any of: /users, /learning-plan, or /incentives endpoints
1. without user token in request header —>  return an error
2. with invalid user token in request header —> return an error
3. with non-string user token in request header —> return an error
4. Validate the response codes for all of the above scenarios

## GET requests to any endpoint besides  /users, /learning-plan, or /incentives —> return an error

## GET requests to  /users endpoint
1. with valid user token —> Returns all active users that work for the company that the querying user works for. Ex: User1 works for Company1, return all users  that work for Company1
2. Validate the response codes for all of the above scenarios

## GET requests to  /learning-plan endpoint
1. with valid user token of a user WITH an “active” learning plan THAT CONTAINS an  “active” plan item —> Returns a list of "active" plan items belonging to that "active" plan for the querying user
2. with valid user token of a user WITH an “active” learning plan that does NOT contain an “active” plan item —> Returns a message no active learning plan items exist for this user. TODO: Find out if an empty list is preferred over the message.
3. with valid user token of a user WITH AT LEAST ONE “completed” or “deleted” learning plan, but NO “active” learning plans —> Returns a message that no active learning plans exist for this user. TODO: Find out if an empty list is preferred over the message.
4. with valid user token of a user WITHOUT learning plans —> Returns a message that no active learning plans exist for this user. TODO: Find out if an empty list is preferred over the messge.
5. Validate the response codes for all of the above scenarios

## GET requests to  /incentives endpoint
1. with valid user token of a user WITHOUT a “completed” learning plan —> Returns a message that no completed learning plans exist for this user. TODO: Find out if an empty list is preferred over the message.
2. with valid user token of a user WITH a “completed” learning plan —> returns a list plan items that meet all of the following conditions: 
    - that are active
    - is an incentive
    - the incentive’s company IDs match the user’s company ID
    - the users’s role matches the role of the incentive
    - users tenure (days) are greater than the incentive’s service requirement (days)  
NOTE: If any of the above 5 are not true, that plan item should not be part of the returned list

3. Validate the response codes for all of the above scenarios

# Frameworks
NUnit - provides Unit and API testing options


