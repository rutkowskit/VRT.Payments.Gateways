# Dev notes


### CI/CD

Add two secrets on github:
1. *NUGET_API_KEY* with properly generated nuget api key
1. *NUGET_SOURCE* - destination nuget source. `https://api.nuget.org/v3/index.json`

These values are required by publishing workflow.


### Release build

```
dotnet publish -c Release
```

### Version management

Add git tag with new version number:

`git tag v8.0.4`

If you are building packages on github, then push the tag:
`git push --tags`
The `Release.yml` workflow will build, test and publish package to nuget.

Build using release configuration:

`dotnet build -c Release`

The build command will also generate nuget package with the version eqal to the tag.

