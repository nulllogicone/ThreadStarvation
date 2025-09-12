# Thread starvation

This repository comes from a good presentation of Damian Edwards and David Fowler (two awesome guys from the .Net Core team) about Diagnosing issues in your code.

Simple story:

## Threading

A little Hello world backend/web/app/server/ with 4 endpoints

- Good old full synchronous execution down the line, works but not very efficient **sync-over-sync**  
- Modern app with legacy services: Not best but works at least not worse than before **async-over-sync**  
- Old code using modern apis runs into dead locks in backend ☠ (IIS , kestrel, ngix)  because of thread starvation :-(   **sync-over-async (☠)**
- Full async, unlimited web scale **async-over-async** (:-))

## Requestor

Makes as may requests against an enpoint as you like!
Just press key arrow up/down for parallel request threads.
You can even start multiple requestor-clients in different terminals against different endpoints.  

[Source code]  
https://github.com/davidfowl/NdcLondon2018

[Youtube]
https://www.youtube.com/watch?v=RYI0DHoIVaA

## Howto to run it

- Open the repository folder in VS Code (or GitHub Codespace)
- Open two terminal windows (CTRL+`)
- Navigate in the left to ~/Requestor
- Navigate in the right to ~/Threading 

Then you can `dotnet run` the web application in the right terminal and it shows Available threads and handled requests.
On the left side you can `dotnet run <method name>` where method name is one of the following

|dotnet run \<parameter> | comment|
|---------|---|
|hello                    | good old sync-over-sync |
|hello-async-over-sync    | modern controller over old services |
|hello-sync-over-async    | ☠ can kill the server, must close terminal |
|hello-async              | scales!|

![VSC](https://raw.githubusercontent.com/nulllogicone/ThreadStarvation/master/images/VS_Code.PNG)
