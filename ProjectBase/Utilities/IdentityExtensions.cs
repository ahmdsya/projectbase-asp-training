using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Optimization;

public static class IdentityExtensions
{
    public static string GetUserDataByKey(this IIdentity identity, string key)
    {
        if (identity == null)
        {
            throw new ArgumentNullException("identity");
        }
        var ci = (ClaimsIdentity)identity;
        if (ci != null)
        {
            try
            {
                return ci.FindFirst(key).Value;
            }
            catch { }
        }
        return null;
    }

    public static IEnumerable<T> OrderBySequence<T, TId>(
       this IEnumerable<T> source,
       IEnumerable<TId> order,
       Func<T, TId> idSelector)
    {
        var lookup = source.ToLookup(idSelector, t => t);
        foreach (var id in order)
        {
            foreach (var t in lookup[id])
            {
                yield return t;
            }
        }
    }

}
public class NonOrderingBundleOrderer : IBundleOrderer
{
    public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
    {
        return files;
    }
}