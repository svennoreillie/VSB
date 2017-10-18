# Soc-Core Starter Project
## What?
This is a tool for starting a new project contain the soc-core style in an Angular application served from a ASP.NET Core backend. 

## Why?
Using a starting project results in a consistent folder strucure, feature set and helps in creating a fully functioning web application in less time. 

## How?
For now you can just git clone this repository and start working straight away. You will need to adjust some names for the title of the application but modification will be minor. A yeoman (or equivalent) template is planned to ease this process even more.

## Prerequisites
Probably not, there are some prerequisites you need to have and some that are just handy to also have globally.
- Git (obviously) => test by running 'git'
- Dotnet (install for dot.net website) => test by running 'dotnet'
- Node and npm (install this from the nodejs.org website) => test by running 'node -v'
- Update npm 'npm install -g npm@latest' => test by running 'npm -v'
- Webpack 'npm install -g webpack'  =>  test by running 'webpack'
- Angular CLI 'npm install -g @angular/cli'  => test by running 'ng'

## Will it work out of the box?
No, you will need to run 'dotnet restore', 'npm install', 'webpack' and 'dotnet run'
To run in development environment (and have all sorts of handy features such as Hot module replacement and automatic reloading enabled) set the environment to Development by running 'set ASPNETCORE_ENVIRONMENT=Development' on a Windows machine or 'export ASPNETCORE_ENVIRONMENT=Development' on macOS. ASP.NET Core enables multiple ways to set variables, check [the docs](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration?tabs=basicconfiguration) for more information on this topic.


## Can I do this myself?
Sure! We started this project by running 'dotnet new angular' and started adjusting.
We altered the webpack files to include the extract-text-webpack-plugin for creating our css file. We also added a rule in webpack to read scss files and put them trough sass-loader and css-loader first. Don't forget to add the npm packages required for these rules.

We added a the soc-core package to the project and replaced the bootstrap import with a soc-core import in the boot.browser.ts file. Also we imported a custom Sass file that references the soc-core.scss file and alters it with some template variables. To see how this is done, please refer to the soc-core installation manual. 

Finally, we changed the layout.cshtml file and started rebuilding the predefined template to create a basic layout that uses the soc-core elements. Refer to the soc-core examples to see how to do this.