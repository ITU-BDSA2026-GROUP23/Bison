// Dafny program FilterWrapper.dfy compiled into C#
// To recompile, you will need the libraries
//     System.Runtime.Numerics.dll System.Collections.Immutable.dll
// but the 'dotnet' tool in .NET should pick those up automatically.
// Optionally, you may want to include compiler switches like
//     /debug /nowarn:162,164,168,183,219,436,1717,1718

using System;
using System.Numerics;
using System.Collections;
[assembly: DafnyAssembly.DafnySourceAttribute(@"// dafny 4.11.0.0
// Command-line arguments: translate cs Bison.Dafny/FilterWrapper.dfy --allow-external-contracts --output Bison.Dafny/GeneratedFilter.cs
// FilterWrapper.dfy


module FilterWrapper {
  function FilterObservations(root: Taxon, observations: seq<Observation>): seq<Observation>
    ensures FilterObservations(root, observations) == FilterBy(root, observations)
    decreases root, observations
  {
    FilterBy(root, observations)
  }

  import opened Models

  import opened BisonFilter
}

module Models {
  class {:extern ""Taxon""} Taxon {
    function {:extern} isSubTaxon(ancestor: Taxon): bool
      decreases ancestor
  }

  class {:extern ""Observation""} Observation {
    function {:extern} getTaxon(): Taxon
  }
}

module BisonFilter {
  function ExpectedFilter(root: Taxon, obs: seq<Observation>): seq<Observation>
    decreases |obs|
  {
    if |obs| == 0 then
      []
    else if obs[0].getTaxon() == root || obs[0].getTaxon().isSubTaxon(root) then
      [obs[0]] + ExpectedFilter(root, obs[1..])
    else
      ExpectedFilter(root, obs[1..])
  }

  function FilterBy(root: Taxon, obs: seq<Observation>): seq<Observation>
    ensures forall o: Observation {:trigger o.getTaxon()} {:trigger o in FilterBy(root, obs)} :: o in FilterBy(root, obs) ==> o.getTaxon() == root || o.getTaxon().isSubTaxon(root)
    ensures forall o: Observation {:trigger o in FilterBy(root, obs)} {:trigger o.getTaxon()} {:trigger o in obs} :: o in obs && (o.getTaxon() == root || o.getTaxon().isSubTaxon(root)) ==> o in FilterBy(root, obs)
    ensures FilterBy(root, obs) == ExpectedFilter(root, obs)
    decreases |obs|
  {
    if |obs| == 0 then
      []
    else
      var current: Observation := obs[0]; var remaining: seq<Observation> := FilterBy(root, obs[1..]); if current.getTaxon() == root || current.getTaxon().isSubTaxon(root) then [current] + remaining else remaining
  }

  import opened Models
}
")]

namespace Dafny {
  internal class ArrayHelpers {
    public static T[] InitNewArray1<T>(T z, BigInteger size0) {
      int s0 = (int)size0;
      T[] a = new T[s0];
      for (int i0 = 0; i0 < s0; i0++) {
        a[i0] = z;
      }
      return a;
    }
  }
} // end of namespace Dafny
internal static class FuncExtensions {
  public static Func<UResult> DowncastClone<TResult, UResult>(this Func<TResult> F, Func<TResult, UResult> ResConv) {
    return () => ResConv(F());
  }
  public static Func<U, UResult> DowncastClone<T, TResult, U, UResult>(this Func<T, TResult> F, Func<U, T> ArgConv, Func<TResult, UResult> ResConv) {
    return arg => ResConv(F(ArgConv(arg)));
  }
  public static Func<U1, U2, UResult> DowncastClone<T1, T2, TResult, U1, U2, UResult>(this Func<T1, T2, TResult> F, Func<U1, T1> ArgConv1, Func<U2, T2> ArgConv2, Func<TResult, UResult> ResConv) {
    return (arg1, arg2) => ResConv(F(ArgConv1(arg1), ArgConv2(arg2)));
  }
}
// end of class FuncExtensions
namespace Models {



} // end of namespace Models
namespace BisonFilter {

  public partial class __default {
    public static Dafny.ISequence<Models.Observation> ExpectedFilter(Models.Taxon root, Dafny.ISequence<Models.Observation> obs)
    {
      Dafny.ISequence<Models.Observation> _0___accumulator = Dafny.Sequence<Models.Observation>.FromElements();
    TAIL_CALL_START: ;
      if ((new BigInteger((obs).Count)).Sign == 0) {
        return Dafny.Sequence<Models.Observation>.Concat(_0___accumulator, Dafny.Sequence<Models.Observation>.FromElements());
      } else if (((((obs).Select(BigInteger.Zero)).getTaxon()) == (object) (root)) || ((((obs).Select(BigInteger.Zero)).getTaxon()).isSubTaxon(root))) {
        _0___accumulator = Dafny.Sequence<Models.Observation>.Concat(_0___accumulator, Dafny.Sequence<Models.Observation>.FromElements((obs).Select(BigInteger.Zero)));
        Models.Taxon _in0 = root;
        Dafny.ISequence<Models.Observation> _in1 = (obs).Drop(BigInteger.One);
        root = _in0;
        obs = _in1;
        goto TAIL_CALL_START;
      } else {
        Models.Taxon _in2 = root;
        Dafny.ISequence<Models.Observation> _in3 = (obs).Drop(BigInteger.One);
        root = _in2;
        obs = _in3;
        goto TAIL_CALL_START;
      }
    }
    public static Dafny.ISequence<Models.Observation> FilterBy(Models.Taxon root, Dafny.ISequence<Models.Observation> obs)
    {
      if ((new BigInteger((obs).Count)).Sign == 0) {
        return Dafny.Sequence<Models.Observation>.FromElements();
      } else {
        Models.Observation _0_current = (obs).Select(BigInteger.Zero);
        Dafny.ISequence<Models.Observation> _1_remaining = BisonFilter.__default.FilterBy(root, (obs).Drop(BigInteger.One));
        if ((((_0_current).getTaxon()) == (object) (root)) || (((_0_current).getTaxon()).isSubTaxon(root))) {
          return Dafny.Sequence<Models.Observation>.Concat(Dafny.Sequence<Models.Observation>.FromElements(_0_current), _1_remaining);
        } else {
          return _1_remaining;
        }
      }
    }
  }
} // end of namespace BisonFilter
namespace FilterWrapper {

  public partial class __default {
    public static Dafny.ISequence<Models.Observation> FilterObservations(Models.Taxon root, Dafny.ISequence<Models.Observation> observations)
    {
      return BisonFilter.__default.FilterBy(root, observations);
    }
  }
} // end of namespace FilterWrapper
namespace _module {

} // end of namespace _module
