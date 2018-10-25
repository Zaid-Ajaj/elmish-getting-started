#r @"packages/build/FAKE/tools/FakeLib.dll"

open Fake
open System
open System.IO

let applicationPath = "./src"
let dotnetCli = "dotnet"
let cwd = __SOURCE_DIRECTORY__

let run fileName args workingDir =
    printfn "CWD: %s" workingDir
    let fileName, args =
        if isUnix
        then fileName, args else "cmd", ("/C " + fileName + " " + args)
    let ok =
        execProcess (fun info ->
             info.FileName <- fileName
             info.WorkingDirectory <- workingDir
             info.Arguments <- args) TimeSpan.MaxValue
    if not ok then failwith (sprintf "'%s> %s %s' task failed" workingDir fileName args)

let delete file = 
    if File.Exists(file) 
    then File.Delete file
    else () 

let cleanBundles() = 
    Path.Combine("public", "bundle.js") 
        |> Path.GetFullPath 
        |> delete
    Path.Combine("public", "bundle.js.map") 
        |> Path.GetFullPath
        |> delete 

Target "Clean" <| fun _ ->
    CleanDirs [ applicationPath </> "bin" ; applicationPath </> "obj" ]
    cleanBundles()


Target "InstallNpmPackages" <| fun _ ->
  // check node version
  run "node" "--version" cwd
  // check npm version
  run "npm" "--version" cwd
  // install npm packages
  run "npm" "install" cwd

Target "Build" <| fun _ ->
    // restore the project (install packages) 
    run dotnetCli "restore --no-cache" applicationPath
    // run fable compiler to compile the project
    run dotnetCli "fable npm-run build --port free" applicationPath

Target "Watch" <| fun _ ->
    // restore the project (install packages) 
    run dotnetCli "restore --no-cache" applicationPath
    // run fable compiler in watch mode
    run dotnetCli "fable npm-run start" applicationPath

"Clean"
  ==> "InstallNpmPackages"
  ==> "Build"

"Clean"
  ==> "InstallNpmPackages"
  ==> "Watch"


RunTargetOrDefault "Build"