# Replacer

It is used to recreate domains with the same functions in the Application layer.


    .
    ├── ...
    ├── Users
    │   └── Commands
    │   │   ├── Create
    │   │   │   ├── UserCreateCommand.cs
    │   │   │   └── UserCreateHandler.cs
    │   │   ├── Update
    │   │   │   ├── UserUpdateCommand.cs
    │   │   │   └── UserUpdateHandler.cs
    │   │   └── Delete
    │   │       ├── UserDeleteCommand.cs
    │   │       └── UserDeleteHandler.cs
    │   └── Models
    │         └── UserModel.cs
    └── ...

For example, you can easily convert the Users layer above to the Projects layer below.
    
    .
    ├── ...
    ├── Projects
    │   └── Commands
    │   │   ├── Create
    │   │   │   ├── ProjectCreateCommand.cs
    │   │   │   └── ProjectCreateHandler.cs
    │   │   ├── Update
    │   │   │   ├── ProjectUpdateCommand.cs
    │   │   │   └── ProjectUpdateHandler.cs
    │   │   └── Delete
    │   │       ├── ProjectDeleteCommand.cs
    │   │       └── ProjectDeleteHandler.cs
    │   └── Models
    │         └── ProjectModel.cs
    └── ...
