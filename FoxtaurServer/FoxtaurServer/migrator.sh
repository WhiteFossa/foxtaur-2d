#!/bin/bash

dotnet ef database update --context SecurityDbContext
dotnet ef database update --context MainDbContext

exit 0
