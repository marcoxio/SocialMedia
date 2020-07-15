# Example Social Media RestFull API

# Nuget Package Installing

## Tools
`dotnet add package Microsoft.EntityFrameworkCore.Tools --version 3.1.3`

## SqlServer
`dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 3.1.3`

# Model Database First(Scaffolding)
Use un layer Infrastructure
`dotnet ef dbcontext scaffold "Server=localhost\SQLEXPRESS;Database=SocialMedia;Integrated Security = true;" Microsoft.EntityFrameworkCore.SqlServer -o Data`