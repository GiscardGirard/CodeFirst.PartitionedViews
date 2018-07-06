for /r %%i in (*.nupkg) do ( 
c:\dev\nuget push %%i oy2mke7kntrieaq5vtzqdksb7mnebnqehk5wrdj3yw5cum -src https://api.nuget.org/v3/index.json
)

