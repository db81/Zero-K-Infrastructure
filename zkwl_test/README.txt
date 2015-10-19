This is a proof of concept of integrating swl into zk-infra.

zkwl_test (better name pending) is a ASP.net 5 project, that's required
for npm and grunt integration to work.

Grunt runs as a post-build step and produces zkwl.bundle.js that can be
then linked (as in Add -> Existing file -> Add as link) from other projects.

Need to make sure changing project files like app.jsx causes the project to be
rebuilt.

If build fails saying "The Dnx Runtime package needs to be installed", you need
to update PowerShell (http://www.microsoft.com/en-us/download/details.aspx?id=40855).