module Models {
    class {:extern "Taxon"} Taxon {
        function {:extern} isSubTaxon(ancestor: Taxon): bool
    }

    class {:extern "Observation"} Observation {
        function {:extern} getTaxon(): Taxon
    }
}

module BisonFilter {

    import opened Models

    function ExpectedFilter(root: Taxon, obs: seq<Observation>): seq<Observation>
    decreases |obs|
    {
    if |obs| == 0 then
        []
    else if obs[0].getTaxon() == root ||
            obs[0].getTaxon().isSubTaxon(root)
    then
        [obs[0]] + ExpectedFilter(root, obs[1..])
    else
        ExpectedFilter(root, obs[1..])
    }

    function FilterBy(root: Taxon, obs: seq<Observation>): seq<Observation>
    ensures forall o :: (o in FilterBy(root, obs)) ==>
        (o.getTaxon() == root || o.getTaxon().isSubTaxon(root))

    ensures forall o ::
        ((o in obs) && (o.getTaxon() == root || o.getTaxon().isSubTaxon(root))) ==>
        (o in FilterBy(root, obs))

    ensures FilterBy(root, obs) == ExpectedFilter(root, obs)

    decreases |obs|
    {
    if |obs| == 0 then
        []
    else
        var current := obs[0];
        var remaining := FilterBy(root, obs[1..]);

        if current.getTaxon() == root ||
        current.getTaxon().isSubTaxon(root)
        then
        [current] + remaining
        else
        remaining
    }
}