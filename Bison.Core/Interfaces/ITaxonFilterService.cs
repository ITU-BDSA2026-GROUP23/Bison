using Models;

namespace Bison.Core.Interfaces;

public interface ITaxonFilterService
{
    List<Observation> FilterByTaxon(Taxon root,List<Observation> observations);
}