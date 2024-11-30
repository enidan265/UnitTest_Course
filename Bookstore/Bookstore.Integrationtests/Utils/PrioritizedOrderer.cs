using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace Bookstore.Integrationtests.Utils;

public class PrioritizedOrderer : ITestCaseOrderer
{
    public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases) where TTestCase : ITestCase
    {
        var sortedMethods = new SortedDictionary<int, List<ITestCase>>();

        foreach (ITestCase testCase in testCases)
        {
            int priority = 0;

            foreach (IAttributeInfo attribute in testCase.TestMethod
                .Method.GetCustomAttributes(typeof(TestPriorityAttribute).AssemblyQualifiedName))
            {
                priority = attribute.GetNamedArgument<int>("Priority");
            }

            GetOrCreate(sortedMethods, priority).Add(testCase);
        }

        foreach (var list in sortedMethods.Keys.Select(priority => sortedMethods[priority]))
        {
            list.Sort((x, y) => StringComparer.OrdinalIgnoreCase.Compare(x.TestMethod.Method.Name,
                y.TestMethod.Method.Name));

            foreach (TTestCase testCase in list)
                yield return testCase;
        }
    }

    private TValue GetOrCreate<TKey, TValue>
        (IDictionary<TKey, TValue> dictionary, TKey key) where TValue : new()
    {
        TValue result;

        if (dictionary.TryGetValue(key, out result)) return result;

        result = new TValue();
        dictionary[key] = result;

        return result;
    }
}
