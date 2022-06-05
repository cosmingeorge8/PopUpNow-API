# PopUpNow-API

Back-end server intented for PopUpNow-Marketplace application.

## Prerequisites
  - project zip file or cloned from the repository [REPO](https://github.com/cosmingeorge8/PopUpNow-API)
  - have Docker installed on the machine
  - cd into the root folder of the project

Installation guide: 


### 1. Using a terminal, cd into the solution folder

    ```
    cd PopUp-Now\ API
    ```
    The docker-compose.yml file is located in the solution folder.

### 2. Run docker-compose build

    ```
    docker-compose build
    ```
    This will build the docker images (SQLServer and API).

### 3. Run docker-compose up 

    ```
    docker-compose up
    ```
    This will create the containers and start the database and the .NET instance.
    
### 4. Acess API at port 7557
    Use postman or curl to test the endpoints.
