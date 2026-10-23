include "Filter.dfy"

module FilterWrapper {
    import opened Models
    import opened BisonFilter

    function FilterObservations(root: Taxon, observations: seq<Observation>): seq<Observation>
        ensures FilterObservations(root, observations) == FilterBy(root, observations)
    {
        FilterBy(root, observations)
    }
}