# LeaderboardAPI
This is a private repository for a REST leaderboard API written in ASP.NET Core 5.0.</br>

### From the Coding Challenge document:
>The goal of this challenge is to get a better understanding of your service design and implementation abilities. Please use C# and ASP.NET Core (either Visual Code or Visual Studio).

>Create a working RESTful leaderboard API that ultimately will use a Redis Sorted Set for the underlying
leaderboard datastore. Leaderboard entries consist of username, score, and index. Note that Redis
automatically assigns indices in the Sorted Set based on score and then lexicographically and when you
use Redis commands like ZRANGE, entries can be returned from highest to lowest score. For this
challenge, you can focus on implementing a single endpoint: /api/leaderboard
/api/leaderboard

>This should return a JSON document of n leaderboard entries where n is provided by an optional query
string parameter or defaults to a service setting (ideally this is configurable setting in your project). The
leaderboard entries should be returned from highest to lowest score. This endpoint should support
pagination so subsequent calls can be made to return entries beyond the first n entries. The response
JSON document can include page and other properties as you see fit.

>Your API controller project should use dependency injection to include a dependent class that
implements an interface (ex: ILeaderboardService) which has methods for accessing the leaderboard
data (you should define whatever interface methods and properties you think are needed). For the
challenge, you can implement a simple class using the interface that returns static leaderboard data so
you don’t have to implement a class that actually works with Redis. If you’d like, you can include
thoughts on what Redis Sorted Set commands you would use for a Redis integration.

>Please add comments to your code to describe your design thoughts and any assumptions you make.
And if time permits, please include controller unit tests.

### Assumptions and what I have implemented:</br>
Considering the challenge document and the ultimate goal of what I am designing and implementing, I have came up with these design considerations/rules:</br>
1. The client will have the option of providing optional query parameters to receive their paginated response of leaderboard data. </br>
>**For example**: 
/api/leaderboard?page=4&count=50 where page and count are **optional** parameters</br>
2. If the page and count query parameters are empty or not given, then return a leaderboard with a list of the top entries of a default page size.</br>
3. If only the count parameter is given and valid, then return the first page of the leaderboard with a list of the top entries with the count as the size of the list.</br>
4. If only the page parameter is given and valid, then return a leaderboard with a list of the top entries of default page size, **IF and only IF**, the page is within range of 1...N where N is the total number of pages determined by the default page size.</br>
>**For example**:
If there are 200 total entries, and the page requested was 5 and the default page size is 50, then the request is invalid because N is equal to 4 and 5 > 4. It will also be invalid if the page requested is less than 1.
5. If both page and count are given and valid, return the paginated leaderboard with the correct page and entries per page that was requested.</br>
6. For every invalid request, the API will send a **404 Resource Not Found** response.

An example of a successfull JSON response will look similar to this:
```
{
  "entries": [
    {
      "username": "Ratliff",
      "score": 220,
      "index": 0
    }
  ],
  "page": 5,
  "totalPages": 5,
  "pageSize": 50,
  "subsetCount": 1
}
```
Where:
- "entries" is a list of entries with a username, score, and index.
- "page" is the page that was either requested or a default of 1 if not specified.
- "totalPages" is the total number of pages possible from either the specified count query parameter, or default page size.
- "pageSize" is either the specified count query parameter, or the default page size.
- "subsetCount" is the count of entries for the specific page of the response. Notice that this can be different than the page size.

An invalid request will yield this response:
```
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Not Found",
  "status": 404,
  "traceId": "00-dde0e828505fd24a9c45ba600d70524d-f7a70c61f7e5214f-00"
}
```

#### More design thoughts and info:

- Since this challenge is not required to actually implement a class with Redis integration, I have decided to load static data from the "Resources" folder named "leaderboard_data.json".
That means my service/in-memory-repository will be loading that file and deserializing it into a leaderboard record that is defined in the "Models" folder. 

- I believe that optimally, a Redis integrated class could just use a command such as 'ZREVRANGEBYSCORE leaderboardKey start count' and would require less logic to be written in the repository/service classes.

- The service file will then package that processed information into a LeaderboardDto record which will be returned. The LeaderboardDto has additional properties in it that is processed in the service, 
which is in line with the data contract described above with the example response.

- Finally, the controller is dependecy injected with an interface of that service/in-memory-repository mentioned above, so that the repository is only generated once and passed to the controller. 
The default page size is also read from the "appsettings.json" file and is passed to the controller as well.

##### Unit Tests:
Unit tests for the controller are available and is written using xUnit. In the solution explorer, it is inside "LeaderboardAPI.Tests" as "LeaderboardControllerTests.cs". I am using the FakeItEasy package to help mock return data and the repository/service.

### How to run the project:</br>
This project was created in .NET Core 5.0. In order to successfully run the project, please ensure that you have the latest .NET SDK installed and your editor/IDE supports C# 9.0. </br>

1. From the repository, select the "Code" dropdown in green.
![image](https://user-images.githubusercontent.com/33888287/155455232-25f644ac-7378-45b6-8b37-0dc2b6262aad.png)</br>

You can choose to copy and paste that URL into the "Git --> Clone Repository" option in Visual Studio and hit "Clone".</br>
![image](https://user-images.githubusercontent.com/33888287/155455395-b85188b7-9b01-410b-a926-67ba005b7a1e.png)</br>
Or simply download the project as a .zip and extract it to a folder of your choice. When that is complete, open the file that is called "LeaderboardAPI.sln".

2. Once the project is open in Visual Studio, you can run the Web API by selecting the "LeaderboardAPI" project in the dropdown and then hitting the green play button.
![image](https://user-images.githubusercontent.com/33888287/155456063-e8698209-cd00-4d81-a42a-f7e0f2e66001.png)</br>

3. Finally, a browser window should open to the project's Swagger url "https://localhost:{PORT}/swagger/index.html":
![image](https://user-images.githubusercontent.com/33888287/155456264-83e9d2e9-a2ff-46f4-876d-3c775815bf2c.png)

4. From here, you are able to easily test the API after clicking on the "Try it out" button:
![image](https://user-images.githubusercontent.com/33888287/155456478-5c4b55ed-d43f-41e5-a799-ce8d851a0158.png)

5. Your responses should look like this after hitting "Execute" if your parameters are valid:
![image](https://user-images.githubusercontent.com/33888287/155456591-ce0bb29b-f79a-4419-a237-c7cf890b7ad0.png)

### How to run the controller unit tests:</br>
1. Simply navigate to "Test" --> "Test Explorer":</br>
![image](https://user-images.githubusercontent.com/33888287/155462676-5067d0ca-b99f-445f-9167-1d4bb9e9768b.png)</br>
2. Click on the double play button:</br>
![image](https://user-images.githubusercontent.com/33888287/155462806-ecbeee8c-11bd-4bb7-b479-6bcdae6e17df.png)


