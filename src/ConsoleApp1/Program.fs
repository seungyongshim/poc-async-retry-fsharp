open AsyncExtensions
open System
open FsToolkit.ErrorHandling.AsyncResultCE
open FsToolkit.ErrorHandling

exception TestError

let x = asyncResult {
    printfn "Hello from F#"
    Result.mapError raise TestError
}

retry 9 x |> Async.RunSynchronously

//retry 9 x |> Async.AwaitTask |> Async.RunSynchronously

