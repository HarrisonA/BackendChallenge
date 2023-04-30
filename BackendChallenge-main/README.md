# Backend Challenge Starter
This is a template project to save you the hassle of having to set up your project from scratch. 
You're free to change it however you want or start from scratch. Whatever is easiest for you.

# Running the project
_assumes you have .NET Core 6 installed_
1. Navigate to the backend challenge folder
```
cd ./BackendChallenge
```
2. Run the project
```
dotnet run
```
3. Verify it's working by navigating to this endpoint in your browser (you may need to change the port based on what address is shown when you run project).
It should display the text "working".
```
https://localhost:7076/users
```


## TO DO list:
2. The repository should have a README.md file at the root with the following information 
3. Write tests plan for all endpoints
4. (DONE) Update model and Create a migration file to add LearningItemName and ItemId to the learningPlanItem model and DBtable. DONE - Using:  
  `dotnet ef migrations add addNewColumnsToLearningPlanItemTable` and 
  `dotnet ef database update` 
  NOTE: TO UNDO, use `ef migrations remove`


## How to run the code:
1. From the terminal:  dotnet run
2. Make GET request using Postman and include as a key in the request headers.
(Ex: key -> UserToken   value -> 1DeyjK5vvSwjc9o9jYArVo2yov2SnjnXEE)


## Questions to turn in:

  How many active users are there for UserToken = 1MEYQDDgwrTkYPtu7Vhfyjp7qkuGnf4ztR company?  
  Answer: 4



  What does the JSON of the user plan for UserToken = 1F7Xg1CJdffsnv9uEXj6GhLERQSam4xwx6 look like?
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

How many incentives is UserToken = 1DeyjK5vvSwjc9o9jYArVo2yov2SnjnXEE eligible for? 
Answer: 0 (Reason: That user does not have, incentives associated with their plan).

