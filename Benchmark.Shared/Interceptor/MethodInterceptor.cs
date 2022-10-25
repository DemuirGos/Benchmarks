using System;
using System.Threading.Tasks;
using PostSharp.Aspects;
using PostSharp.Serialization;

using Benchmarks.Models;

namespace Benchmarks.Extensions;

[PSerializable]
public class OnGeneralMethodBoundaryAspect : MethodInterceptionAspect
{
    public virtual MetricsMetadata OnStarting(MethodInterceptionArgs args) => args;
    public virtual void OnCompletion(MetricsMetadata args) { }
    public virtual void OnSuccess(MetricsMetadata args) { }
    public virtual void OnFailure(MetricsMetadata args) { }

    public override void OnInvoke(MethodInterceptionArgs executionArgs)
    {
#if MONITOR
        MetricsMetadata args = OnStarting(executionArgs);
        args.Status = MethodStatus.OnGoing;
        try
        {
            executionArgs.Proceed();
            args.Status = MethodStatus.Succeeded;
            args.Return = executionArgs.ReturnValue;
            OnSuccess(args);
            return;
        }
        catch (Exception e)
        {
            args.Status = MethodStatus.Failed;
            args.Exception = e;
            OnFailure(args);
            throw;
        }
        finally {
            args.Status |= MethodStatus.Completed;
            OnCompletion(args);
        }
#else
        executionArgs.Proceed();
#endif
    }

    public override async Task OnInvokeAsync(MethodInterceptionArgs executionArgs)
    {
#if MONITOR
        MetricsMetadata args = OnStarting(executionArgs);
        args.Status = MethodStatus.OnGoing;
        try
        {
            await executionArgs.ProceedAsync();
            args.Status = MethodStatus.Succeeded;
            OnSuccess(args);
            return;
        }
        catch (Exception e)
        {
            args.Status = MethodStatus.Failed;
            args.Exception = e;
            OnFailure(args);
            throw;
        }
        finally {
            args.Status |= MethodStatus.Completed;
            OnCompletion(args);
        }
#else
        await executionArgs.ProceedAsync();
#endif
    }
}