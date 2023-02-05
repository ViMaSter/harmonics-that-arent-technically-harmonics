// Heptatonic scales
// Major steps:    {1,   1, 0.5, 1,   1, 1, 0.5}
// Major interval: {0,   2,   4, 5,   7, 9,  11, 12}
// Minor steps:    {1, 0.5,   1, 1, 0.5, 1,   1}
// Minor interval: {0,   2,   3, 5,   7, 8,  10, 12}

// find available sets of 7 intervals that sum up to 6 based on some rules:
    // no more than 1 sets of 3 halfstep jumps in a row
    // no jumps bigger or equal to 4 halfstep jumps
var uniqueVariation = new HashSet<List<decimal>>();
for (decimal positiveOffset = 0; positiveOffset <= 1; positiveOffset+=0.5m)
{
    for (decimal negativeOffset = -1; negativeOffset >= -2; negativeOffset-=0.5m)
    {
        var variation = new List<decimal> { };
        if (negativeOffset + positiveOffset != -1)
        {
            continue;
        }
        
        for (decimal i = 0; i < Math.Abs(negativeOffset); i+=0.5m)
        {
            variation.Add(-0.5m);
        }
        for (decimal i = 0; i < Math.Abs(positiveOffset); i+=0.5m)
        {
            variation.Add(0.5m);
        }

        var fill = 7-variation.Count;
        for (var i = 0; i < fill; i++)
        {
            variation.Add(0);
        }

        // generate all possible combinations of the variation
        Blah.GetPer(variation.ToArray());
        foreach (var decimals in Blah.a)
        {
            uniqueVariation.Add(decimals);
        }
    }    
}

var uniqueVariationsAsStrings = uniqueVariation.Select(a => string.Join("|", a));
var distinctVariationsAsStrings = uniqueVariationsAsStrings.Distinct();
var distinctVariationsAsEnumerable = distinctVariationsAsStrings.Select(s => s.Split("|").Select(decimal.Parse)).ToList();
var readableVariationsAsEnumerable = distinctVariationsAsEnumerable.Select(list => list.Select(entry => (int)(entry * 2 + 2)).ToList()).ToList();
var readableVariationsAsEnumerableWithoutThreeHalfsteps = readableVariationsAsEnumerable.Where(list =>
{
    // skip list if there are two "3"s in a row
    var listAsString = string.Join(",", list.Select(v=>(int)v));
    if(listAsString[0] == '3' && listAsString[^1] == '3')
    {
        return false;
    }

    if (listAsString.Contains("3,3"))
    {
        return false;
    }

    return true;
}).ToList();

List<Tuple<int[], int[]>> relatedScales = new List<Tuple<int[], int[]>>();
foreach (var variationA in readableVariationsAsEnumerableWithoutThreeHalfsteps)
{
    foreach (var variationB in readableVariationsAsEnumerableWithoutThreeHalfsteps)
    {
        for (int i = 0; i < variationA.Count(); i++)
        {
            // convert variations to string and continue if they are the same
            var variationAAsString = string.Join(",", variationA);
            var variationBAsString = string.Join(",", variationB);
            if (variationAAsString == variationBAsString)
            {
                continue;
            }

            var a = variationA.ToList();
            var b = variationB.ToList();
            var c = a.Skip(i).Concat(a.Take(i)).ToList();
            if (c.SequenceEqual(b))
            {
                relatedScales.Add(new Tuple<int[], int[]>(variationA.ToArray(), variationB.ToArray()));
            }            
        }
    }
}

Console.WriteLine($"Total: {readableVariationsAsEnumerableWithoutThreeHalfsteps.Count()}");
foreach (var variation in readableVariationsAsEnumerableWithoutThreeHalfsteps)
{
    var variationAsList = variation.ToList();

    bool FindAsString(Tuple<int[], int[]> tuple)
    {
        var item1AsString = string.Join(",", tuple.Item1);
        var item2AsString = string.Join(",", tuple.Item2);
        var variationAsListAsString = string.Join(",", variationAsList);
        return item1AsString == variationAsListAsString || item2AsString == variationAsListAsString;
    }

    var index = relatedScales.FindAll(FindAsString);
    Console.WriteLine($"Items: {string.Join(",", variationAsList)} (Sum: {variationAsList.Sum()})");
}

Console.WriteLine("relatedScale count: " + relatedScales.Count);