module AsyncRetry
open System.Threading.Tasks

//https://livebook.manning.com/book/concurrency-in-dot-net/chapter-9/96

type ASyncRetryBuilder(max, sleepMilliseconds : int) =
    let rec retry n (task:Async<'a>) (continuation: 'a -> Async<'b>) =
        async {
            try
                let! result = task
                let! conResult = continuation result
                return conResult
            with error ->
                if n = 0 then return raise error
                else
                do! Async.Sleep sleepMilliseconds
                return! retry (n-1) task continuation
        }

    member _.ReturnFrom(f) = f
    member _.Return(v) = async { return v }
    member _.Delay(f) = async { return! f() }
    member _.Bind(task:Async<'a>, continuation: 'a -> Async<'b>) =
        retry max task continuation
    member _.Bind(t : Task, f: unit -> Async<'r>) : Async<'r> =
        async.Bind(Async.AwaitTask t, f)


