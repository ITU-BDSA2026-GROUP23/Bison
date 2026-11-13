using Models;
using Bison.Core.Interfaces;

public class TaxonFilterService : ITaxonFilterService
{
    public List<Observation> FilterByTaxon(Taxon root,List<Observation> observations){
        var dafnyObservations = Dafny.Sequence<Observation>.FromElements(observations.ToArray());

        var result = FilterWrapper.__default.FilterObservations(root,dafnyObservations);

        return result.ToList();
    }
}