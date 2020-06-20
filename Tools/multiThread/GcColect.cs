using System;

public static class GcCollect
{
    public static void gc()
    {
        Loom.RunAsync(() =>
        {
            GC.Collect();
        });
    }
}
