// Copyright (c) Samuel McAravey
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.


using System.ComponentModel;
using System.Globalization;
using CommunityToolkit.Diagnostics;

namespace Bravellian;

[JsonConverter(typeof(UsaStateJsonConverter))]
[TypeConverter(typeof(UsaStateTypeConverter))]
public readonly partial record struct UsaState
        : IComparable,
          IComparable<UsaState>,
          IEquatable<UsaState>,
          IParsable<UsaState>
{
    public const string AlabamaValue = "AL";
    public const string AlaskaValue = "AK";
    public const string ArizonaValue = "AZ";
    public const string ArkansasValue = "AR";
    public const string CaliforniaValue = "CA";
    public const string ColoradoValue = "CO";
    public const string ConnecticutValue = "CT";
    public const string DelawareValue = "DE";
    public const string DistrictOfColumbiaValue = "DC";
    public const string FloridaValue = "FL";
    public const string GeorgiaValue = "GA";
    public const string HawaiiValue = "HI";
    public const string IdahoValue = "ID";
    public const string IllinoisValue = "IL";
    public const string IndianaValue = "IN";
    public const string IowaValue = "IA";
    public const string KansasValue = "KS";
    public const string KentuckyValue = "KY";
    public const string LouisianaValue = "LA";
    public const string MaineValue = "ME";
    public const string MontanaValue = "MT";
    public const string NebraskaValue = "NE";
    public const string NevadaValue = "NV";
    public const string NewHampshireValue = "NH";
    public const string NewJerseyValue = "NJ";
    public const string NewMexicoValue = "NM";
    public const string NewYorkValue = "NY";
    public const string NorthCarolinaValue = "NC";
    public const string NorthDakotaValue = "ND";
    public const string OhioValue = "OH";
    public const string OklahomaValue = "OK";
    public const string OregonValue = "OR";
    public const string MarylandValue = "MD";
    public const string MassachusettsValue = "MA";
    public const string MichiganValue = "MI";
    public const string MinnesotaValue = "MN";
    public const string MississippiValue = "MS";
    public const string MissouriValue = "MO";
    public const string PennsylvaniaValue = "PA";
    public const string RhodeIslandValue = "RI";
    public const string SouthCarolinaValue = "SC";
    public const string SouthDakotaValue = "SD";
    public const string TennesseeValue = "TN";
    public const string TexasValue = "TX";
    public const string UtahValue = "UT";
    public const string VermontValue = "VT";
    public const string VirginiaValue = "VA";
    public const string WashingtonValue = "WA";
    public const string WestVirginiaValue = "WV";
    public const string WisconsinValue = "WI";
    public const string WyomingValue = "WY";
    public const string AlabamaDisplayName = "Alabama";
    public const string AlaskaDisplayName = "Alaska";
    public const string ArizonaDisplayName = "Arizona";
    public const string ArkansasDisplayName = "Arkansas";
    public const string CaliforniaDisplayName = "California";
    public const string ColoradoDisplayName = "Colorado";
    public const string ConnecticutDisplayName = "Connecticut";
    public const string DelawareDisplayName = "Delaware";
    public const string DistrictOfColumbiaDisplayName = "District of Columbia";
    public const string FloridaDisplayName = "Florida";
    public const string GeorgiaDisplayName = "Georgia";
    public const string HawaiiDisplayName = "Hawaii";
    public const string IdahoDisplayName = "Idaho";
    public const string IllinoisDisplayName = "Illinois";
    public const string IndianaDisplayName = "Indiana";
    public const string IowaDisplayName = "Iowa";
    public const string KansasDisplayName = "Kansas";
    public const string KentuckyDisplayName = "Kentucky";
    public const string LouisianaDisplayName = "Louisiana";
    public const string MaineDisplayName = "Maine";
    public const string MontanaDisplayName = "Montana";
    public const string NebraskaDisplayName = "Nebraska";
    public const string NevadaDisplayName = "Nevada";
    public const string NewHampshireDisplayName = "New Hampshire";
    public const string NewJerseyDisplayName = "New Jersey";
    public const string NewMexicoDisplayName = "New Mexico";
    public const string NewYorkDisplayName = "New York";
    public const string NorthCarolinaDisplayName = "North Carolina";
    public const string NorthDakotaDisplayName = "North Dakota";
    public const string OhioDisplayName = "Ohio";
    public const string OklahomaDisplayName = "Oklahoma";
    public const string OregonDisplayName = "Oregon";
    public const string MarylandDisplayName = "Maryland";
    public const string MassachusettsDisplayName = "Massachusetts";
    public const string MichiganDisplayName = "Michigan";
    public const string MinnesotaDisplayName = "Minnesota";
    public const string MississippiDisplayName = "Mississippi";
    public const string MissouriDisplayName = "Missouri";
    public const string PennsylvaniaDisplayName = "Pennsylvania";
    public const string RhodeIslandDisplayName = "Rhode Island";
    public const string SouthCarolinaDisplayName = "South Carolina";
    public const string SouthDakotaDisplayName = "South Dakota";
    public const string TennesseeDisplayName = "Tennessee";
    public const string TexasDisplayName = "Texas";
    public const string UtahDisplayName = "Utah";
    public const string VermontDisplayName = "Vermont";
    public const string VirginiaDisplayName = "Virginia";
    public const string WashingtonDisplayName = "Washington";
    public const string WestVirginiaDisplayName = "West Virginia";
    public const string WisconsinDisplayName = "Wisconsin";
    public const string WyomingDisplayName = "Wyoming";

    public static readonly UsaState Alabama = new(AlabamaValue, AlabamaDisplayName);

    public static readonly UsaState Alaska = new(AlaskaValue, AlaskaDisplayName);

    public static readonly UsaState Arizona = new(ArizonaValue, ArizonaDisplayName);

    public static readonly UsaState Arkansas = new(ArkansasValue, ArkansasDisplayName);

    public static readonly UsaState California = new(CaliforniaValue, CaliforniaDisplayName);

    public static readonly UsaState Colorado = new(ColoradoValue, ColoradoDisplayName);

    public static readonly UsaState Connecticut = new(ConnecticutValue, ConnecticutDisplayName);

    public static readonly UsaState Delaware = new(DelawareValue, DelawareDisplayName);

    public static readonly UsaState DistrictOfColumbia = new(DistrictOfColumbiaValue, DistrictOfColumbiaDisplayName);

    public static readonly UsaState Florida = new(FloridaValue, FloridaDisplayName);

    public static readonly UsaState Georgia = new(GeorgiaValue, GeorgiaDisplayName);

    public static readonly UsaState Hawaii = new(HawaiiValue, HawaiiDisplayName);

    public static readonly UsaState Idaho = new(IdahoValue, IdahoDisplayName);

    public static readonly UsaState Illinois = new(IllinoisValue, IllinoisDisplayName);

    public static readonly UsaState Indiana = new(IndianaValue, IndianaDisplayName);

    public static readonly UsaState Iowa = new(IowaValue, IowaDisplayName);

    public static readonly UsaState Kansas = new(KansasValue, KansasDisplayName);

    public static readonly UsaState Kentucky = new(KentuckyValue, KentuckyDisplayName);

    public static readonly UsaState Louisiana = new(LouisianaValue, LouisianaDisplayName);

    public static readonly UsaState Maine = new(MaineValue, MaineDisplayName);

    public static readonly UsaState Montana = new(MontanaValue, MontanaDisplayName);

    public static readonly UsaState Nebraska = new(NebraskaValue, NebraskaDisplayName);

    public static readonly UsaState Nevada = new(NevadaValue, NevadaDisplayName);

    public static readonly UsaState NewHampshire = new(NewHampshireValue, NewHampshireDisplayName);

    public static readonly UsaState NewJersey = new(NewJerseyValue, NewJerseyDisplayName);

    public static readonly UsaState NewMexico = new(NewMexicoValue, NewMexicoDisplayName);

    public static readonly UsaState NewYork = new(NewYorkValue, NewYorkDisplayName);

    public static readonly UsaState NorthCarolina = new(NorthCarolinaValue, NorthCarolinaDisplayName);

    public static readonly UsaState NorthDakota = new(NorthDakotaValue, NorthDakotaDisplayName);

    public static readonly UsaState Ohio = new(OhioValue, OhioDisplayName);

    public static readonly UsaState Oklahoma = new(OklahomaValue, OklahomaDisplayName);

    public static readonly UsaState Oregon = new(OregonValue, OregonDisplayName);

    public static readonly UsaState Maryland = new(MarylandValue, MarylandDisplayName);

    public static readonly UsaState Massachusetts = new(MassachusettsValue, MassachusettsDisplayName);

    public static readonly UsaState Michigan = new(MichiganValue, MichiganDisplayName);

    public static readonly UsaState Minnesota = new(MinnesotaValue, MinnesotaDisplayName);

    public static readonly UsaState Mississippi = new(MississippiValue, MississippiDisplayName);

    public static readonly UsaState Missouri = new(MissouriValue, MissouriDisplayName);

    public static readonly UsaState Pennsylvania = new(PennsylvaniaValue, PennsylvaniaDisplayName);

    public static readonly UsaState RhodeIsland = new(RhodeIslandValue, RhodeIslandDisplayName);

    public static readonly UsaState SouthCarolina = new(SouthCarolinaValue, SouthCarolinaDisplayName);

    public static readonly UsaState SouthDakota = new(SouthDakotaValue, SouthDakotaDisplayName);

    public static readonly UsaState Tennessee = new(TennesseeValue, TennesseeDisplayName);

    public static readonly UsaState Texas = new(TexasValue, TexasDisplayName);

    public static readonly UsaState Utah = new(UtahValue, UtahDisplayName);

    public static readonly UsaState Vermont = new(VermontValue, VermontDisplayName);

    public static readonly UsaState Virginia = new(VirginiaValue, VirginiaDisplayName);

    public static readonly UsaState Washington = new(WashingtonValue, WashingtonDisplayName);

    public static readonly UsaState WestVirginia = new(WestVirginiaValue, WestVirginiaDisplayName);

    public static readonly UsaState Wisconsin = new(WisconsinValue, WisconsinDisplayName);

    public static readonly UsaState Wyoming = new(WyomingValue, WyomingDisplayName);



    private UsaState([ConstantExpected] string value, [ConstantExpected] string displayName)
    {
        Value = value;
        DisplayName = displayName;
        ProcessValue(value);
    }

    public static IReadOnlySet<UsaState> AllValues { get; } = new HashSet<UsaState>
    {
        Alabama,
        Alaska,
        Arizona,
        Arkansas,
        California,
        Colorado,
        Connecticut,
        Delaware,
        DistrictOfColumbia,
        Florida,
        Georgia,
        Hawaii,
        Idaho,
        Illinois,
        Indiana,
        Iowa,
        Kansas,
        Kentucky,
        Louisiana,
        Maine,
        Montana,
        Nebraska,
        Nevada,
        NewHampshire,
        NewJersey,
        NewMexico,
        NewYork,
        NorthCarolina,
        NorthDakota,
        Ohio,
        Oklahoma,
        Oregon,
        Maryland,
        Massachusetts,
        Michigan,
        Minnesota,
        Mississippi,
        Missouri,
        Pennsylvania,
        RhodeIsland,
        SouthCarolina,
        SouthDakota,
        Tennessee,
        Texas,
        Utah,
        Vermont,
        Virginia,
        Washington,
        WestVirginia,
        Wisconsin,
        Wyoming,
    };

    public string Value { get; init; }

    public string DisplayName { get; init; }

    public static UsaState From(string value) => Parse(value);

    static partial void ProcessValue(string value);

    public override string ToString() => Value;

    public bool Equals(UsaState other)
    {
        return string.Equals(Value, other.Value);
    }

    public override int GetHashCode()
    {
        return Value?.GetHashCode() ?? 0;
    }

    public int CompareTo(UsaState other)
    {
        return string.Compare(Value, other.Value);
    }

    public int CompareTo(object? obj)
    {
        return obj is UsaState id ? Value.CompareTo(id.Value) : Value.CompareTo(obj);
    }

    public static bool operator <(UsaState left, UsaState right) => left.CompareTo(right) < 0;

    public static bool operator <=(UsaState left, UsaState right) => left.CompareTo(right) <= 0;

    public static bool operator >(UsaState left, UsaState right) => left.CompareTo(right) > 0;

    public static bool operator >=(UsaState left, UsaState right) => left.CompareTo(right) >= 0;

    /// <summary>
    /// Matches the current enum value against all possible cases and executes the corresponding delegate.
    /// Throws <see cref="ArgumentOutOfRangeException."/> if no match is found.
    /// </summary>
    /// <param name="caseAlabama">The delegate to execute for the Alabama case.</param>
    /// <param name="caseAlaska">The delegate to execute for the Alaska case.</param>
    /// <param name="caseArizona">The delegate to execute for the Arizona case.</param>
    /// <param name="caseArkansas">The delegate to execute for the Arkansas case.</param>
    /// <param name="caseCalifornia">The delegate to execute for the California case.</param>
    /// <param name="caseColorado">The delegate to execute for the Colorado case.</param>
    /// <param name="caseConnecticut">The delegate to execute for the Connecticut case.</param>
    /// <param name="caseDelaware">The delegate to execute for the Delaware case.</param>
    /// <param name="caseDistrictOfColumbia">The delegate to execute for the DistrictOfColumbia case.</param>
    /// <param name="caseFlorida">The delegate to execute for the Florida case.</param>
    /// <param name="caseGeorgia">The delegate to execute for the Georgia case.</param>
    /// <param name="caseHawaii">The delegate to execute for the Hawaii case.</param>
    /// <param name="caseIdaho">The delegate to execute for the Idaho case.</param>
    /// <param name="caseIllinois">The delegate to execute for the Illinois case.</param>
    /// <param name="caseIndiana">The delegate to execute for the Indiana case.</param>
    /// <param name="caseIowa">The delegate to execute for the Iowa case.</param>
    /// <param name="caseKansas">The delegate to execute for the Kansas case.</param>
    /// <param name="caseKentucky">The delegate to execute for the Kentucky case.</param>
    /// <param name="caseLouisiana">The delegate to execute for the Louisiana case.</param>
    /// <param name="caseMaine">The delegate to execute for the Maine case.</param>
    /// <param name="caseMontana">The delegate to execute for the Montana case.</param>
    /// <param name="caseNebraska">The delegate to execute for the Nebraska case.</param>
    /// <param name="caseNevada">The delegate to execute for the Nevada case.</param>
    /// <param name="caseNewHampshire">The delegate to execute for the NewHampshire case.</param>
    /// <param name="caseNewJersey">The delegate to execute for the NewJersey case.</param>
    /// <param name="caseNewMexico">The delegate to execute for the NewMexico case.</param>
    /// <param name="caseNewYork">The delegate to execute for the NewYork case.</param>
    /// <param name="caseNorthCarolina">The delegate to execute for the NorthCarolina case.</param>
    /// <param name="caseNorthDakota">The delegate to execute for the NorthDakota case.</param>
    /// <param name="caseOhio">The delegate to execute for the Ohio case.</param>
    /// <param name="caseOklahoma">The delegate to execute for the Oklahoma case.</param>
    /// <param name="caseOregon">The delegate to execute for the Oregon case.</param>
    /// <param name="caseMaryland">The delegate to execute for the Maryland case.</param>
    /// <param name="caseMassachusetts">The delegate to execute for the Massachusetts case.</param>
    /// <param name="caseMichigan">The delegate to execute for the Michigan case.</param>
    /// <param name="caseMinnesota">The delegate to execute for the Minnesota case.</param>
    /// <param name="caseMississippi">The delegate to execute for the Mississippi case.</param>
    /// <param name="caseMissouri">The delegate to execute for the Missouri case.</param>
    /// <param name="casePennsylvania">The delegate to execute for the Pennsylvania case.</param>
    /// <param name="caseRhodeIsland">The delegate to execute for the RhodeIsland case.</param>
    /// <param name="caseSouthCarolina">The delegate to execute for the SouthCarolina case.</param>
    /// <param name="caseSouthDakota">The delegate to execute for the SouthDakota case.</param>
    /// <param name="caseTennessee">The delegate to execute for the Tennessee case.</param>
    /// <param name="caseTexas">The delegate to execute for the Texas case.</param>
    /// <param name="caseUtah">The delegate to execute for the Utah case.</param>
    /// <param name="caseVermont">The delegate to execute for the Vermont case.</param>
    /// <param name="caseVirginia">The delegate to execute for the Virginia case.</param>
    /// <param name="caseWashington">The delegate to execute for the Washington case.</param>
    /// <param name="caseWestVirginia">The delegate to execute for the WestVirginia case.</param>
    /// <param name="caseWisconsin">The delegate to execute for the Wisconsin case.</param>
    /// <param name="caseWyoming">The delegate to execute for the Wyoming case.</param>
    /// <exception cref="ArgumentOutOfRangeException.">Thrown when the current value is not handled by any case.</exception>
    public void Match(Action caseAlabama, Action caseAlaska, Action caseArizona, Action caseArkansas, Action caseCalifornia, Action caseColorado, Action caseConnecticut, Action caseDelaware, Action caseDistrictOfColumbia, Action caseFlorida, Action caseGeorgia, Action caseHawaii, Action caseIdaho, Action caseIllinois, Action caseIndiana, Action caseIowa, Action caseKansas, Action caseKentucky, Action caseLouisiana, Action caseMaine, Action caseMontana, Action caseNebraska, Action caseNevada, Action caseNewHampshire, Action caseNewJersey, Action caseNewMexico, Action caseNewYork, Action caseNorthCarolina, Action caseNorthDakota, Action caseOhio, Action caseOklahoma, Action caseOregon, Action caseMaryland, Action caseMassachusetts, Action caseMichigan, Action caseMinnesota, Action caseMississippi, Action caseMissouri, Action casePennsylvania, Action caseRhodeIsland, Action caseSouthCarolina, Action caseSouthDakota, Action caseTennessee, Action caseTexas, Action caseUtah, Action caseVermont, Action caseVirginia, Action caseWashington, Action caseWestVirginia, Action caseWisconsin, Action caseWyoming)
    {
        switch (Value)
        {
            case AlabamaValue:
                caseAlabama();
                return;
            case AlaskaValue:
                caseAlaska();
                return;
            case ArizonaValue:
                caseArizona();
                return;
            case ArkansasValue:
                caseArkansas();
                return;
            case CaliforniaValue:
                caseCalifornia();
                return;
            case ColoradoValue:
                caseColorado();
                return;
            case ConnecticutValue:
                caseConnecticut();
                return;
            case DelawareValue:
                caseDelaware();
                return;
            case DistrictOfColumbiaValue:
                caseDistrictOfColumbia();
                return;
            case FloridaValue:
                caseFlorida();
                return;
            case GeorgiaValue:
                caseGeorgia();
                return;
            case HawaiiValue:
                caseHawaii();
                return;
            case IdahoValue:
                caseIdaho();
                return;
            case IllinoisValue:
                caseIllinois();
                return;
            case IndianaValue:
                caseIndiana();
                return;
            case IowaValue:
                caseIowa();
                return;
            case KansasValue:
                caseKansas();
                return;
            case KentuckyValue:
                caseKentucky();
                return;
            case LouisianaValue:
                caseLouisiana();
                return;
            case MaineValue:
                caseMaine();
                return;
            case MontanaValue:
                caseMontana();
                return;
            case NebraskaValue:
                caseNebraska();
                return;
            case NevadaValue:
                caseNevada();
                return;
            case NewHampshireValue:
                caseNewHampshire();
                return;
            case NewJerseyValue:
                caseNewJersey();
                return;
            case NewMexicoValue:
                caseNewMexico();
                return;
            case NewYorkValue:
                caseNewYork();
                return;
            case NorthCarolinaValue:
                caseNorthCarolina();
                return;
            case NorthDakotaValue:
                caseNorthDakota();
                return;
            case OhioValue:
                caseOhio();
                return;
            case OklahomaValue:
                caseOklahoma();
                return;
            case OregonValue:
                caseOregon();
                return;
            case MarylandValue:
                caseMaryland();
                return;
            case MassachusettsValue:
                caseMassachusetts();
                return;
            case MichiganValue:
                caseMichigan();
                return;
            case MinnesotaValue:
                caseMinnesota();
                return;
            case MississippiValue:
                caseMississippi();
                return;
            case MissouriValue:
                caseMissouri();
                return;
            case PennsylvaniaValue:
                casePennsylvania();
                return;
            case RhodeIslandValue:
                caseRhodeIsland();
                return;
            case SouthCarolinaValue:
                caseSouthCarolina();
                return;
            case SouthDakotaValue:
                caseSouthDakota();
                return;
            case TennesseeValue:
                caseTennessee();
                return;
            case TexasValue:
                caseTexas();
                return;
            case UtahValue:
                caseUtah();
                return;
            case VermontValue:
                caseVermont();
                return;
            case VirginiaValue:
                caseVirginia();
                return;
            case WashingtonValue:
                caseWashington();
                return;
            case WestVirginiaValue:
                caseWestVirginia();
                return;
            case WisconsinValue:
                caseWisconsin();
                return;
            case WyomingValue:
                caseWyoming();
                return;
            default:
                throw new InvalidOperationException($"Unhandled state: {Value}");
        }
    }

    /// <summary>
    /// Matches the current enum value against all possible cases and returns the result of executing the corresponding delegate.
    /// Throws <see cref="ArgumentOutOfRangeException."/> if no match is found.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <param name="caseAlabama">The delegate to execute for the Alabama case.</param>
    /// <param name="caseAlaska">The delegate to execute for the Alaska case.</param>
    /// <param name="caseArizona">The delegate to execute for the Arizona case.</param>
    /// <param name="caseArkansas">The delegate to execute for the Arkansas case.</param>
    /// <param name="caseCalifornia">The delegate to execute for the California case.</param>
    /// <param name="caseColorado">The delegate to execute for the Colorado case.</param>
    /// <param name="caseConnecticut">The delegate to execute for the Connecticut case.</param>
    /// <param name="caseDelaware">The delegate to execute for the Delaware case.</param>
    /// <param name="caseDistrictOfColumbia">The delegate to execute for the DistrictOfColumbia case.</param>
    /// <param name="caseFlorida">The delegate to execute for the Florida case.</param>
    /// <param name="caseGeorgia">The delegate to execute for the Georgia case.</param>
    /// <param name="caseHawaii">The delegate to execute for the Hawaii case.</param>
    /// <param name="caseIdaho">The delegate to execute for the Idaho case.</param>
    /// <param name="caseIllinois">The delegate to execute for the Illinois case.</param>
    /// <param name="caseIndiana">The delegate to execute for the Indiana case.</param>
    /// <param name="caseIowa">The delegate to execute for the Iowa case.</param>
    /// <param name="caseKansas">The delegate to execute for the Kansas case.</param>
    /// <param name="caseKentucky">The delegate to execute for the Kentucky case.</param>
    /// <param name="caseLouisiana">The delegate to execute for the Louisiana case.</param>
    /// <param name="caseMaine">The delegate to execute for the Maine case.</param>
    /// <param name="caseMontana">The delegate to execute for the Montana case.</param>
    /// <param name="caseNebraska">The delegate to execute for the Nebraska case.</param>
    /// <param name="caseNevada">The delegate to execute for the Nevada case.</param>
    /// <param name="caseNewHampshire">The delegate to execute for the NewHampshire case.</param>
    /// <param name="caseNewJersey">The delegate to execute for the NewJersey case.</param>
    /// <param name="caseNewMexico">The delegate to execute for the NewMexico case.</param>
    /// <param name="caseNewYork">The delegate to execute for the NewYork case.</param>
    /// <param name="caseNorthCarolina">The delegate to execute for the NorthCarolina case.</param>
    /// <param name="caseNorthDakota">The delegate to execute for the NorthDakota case.</param>
    /// <param name="caseOhio">The delegate to execute for the Ohio case.</param>
    /// <param name="caseOklahoma">The delegate to execute for the Oklahoma case.</param>
    /// <param name="caseOregon">The delegate to execute for the Oregon case.</param>
    /// <param name="caseMaryland">The delegate to execute for the Maryland case.</param>
    /// <param name="caseMassachusetts">The delegate to execute for the Massachusetts case.</param>
    /// <param name="caseMichigan">The delegate to execute for the Michigan case.</param>
    /// <param name="caseMinnesota">The delegate to execute for the Minnesota case.</param>
    /// <param name="caseMississippi">The delegate to execute for the Mississippi case.</param>
    /// <param name="caseMissouri">The delegate to execute for the Missouri case.</param>
    /// <param name="casePennsylvania">The delegate to execute for the Pennsylvania case.</param>
    /// <param name="caseRhodeIsland">The delegate to execute for the RhodeIsland case.</param>
    /// <param name="caseSouthCarolina">The delegate to execute for the SouthCarolina case.</param>
    /// <param name="caseSouthDakota">The delegate to execute for the SouthDakota case.</param>
    /// <param name="caseTennessee">The delegate to execute for the Tennessee case.</param>
    /// <param name="caseTexas">The delegate to execute for the Texas case.</param>
    /// <param name="caseUtah">The delegate to execute for the Utah case.</param>
    /// <param name="caseVermont">The delegate to execute for the Vermont case.</param>
    /// <param name="caseVirginia">The delegate to execute for the Virginia case.</param>
    /// <param name="caseWashington">The delegate to execute for the Washington case.</param>
    /// <param name="caseWestVirginia">The delegate to execute for the WestVirginia case.</param>
    /// <param name="caseWisconsin">The delegate to execute for the Wisconsin case.</param>
    /// <param name="caseWyoming">The delegate to execute for the Wyoming case.</param>
    /// <returns>The result of executing the matching delegate.</returns>
    /// <exception cref="ArgumentOutOfRangeException.">Thrown when the current value is not handled by any case.</exception>
    public T Match<T>(Func<T> caseAlabama, Func<T> caseAlaska, Func<T> caseArizona, Func<T> caseArkansas, Func<T> caseCalifornia, Func<T> caseColorado, Func<T> caseConnecticut, Func<T> caseDelaware, Func<T> caseDistrictOfColumbia, Func<T> caseFlorida, Func<T> caseGeorgia, Func<T> caseHawaii, Func<T> caseIdaho, Func<T> caseIllinois, Func<T> caseIndiana, Func<T> caseIowa, Func<T> caseKansas, Func<T> caseKentucky, Func<T> caseLouisiana, Func<T> caseMaine, Func<T> caseMontana, Func<T> caseNebraska, Func<T> caseNevada, Func<T> caseNewHampshire, Func<T> caseNewJersey, Func<T> caseNewMexico, Func<T> caseNewYork, Func<T> caseNorthCarolina, Func<T> caseNorthDakota, Func<T> caseOhio, Func<T> caseOklahoma, Func<T> caseOregon, Func<T> caseMaryland, Func<T> caseMassachusetts, Func<T> caseMichigan, Func<T> caseMinnesota, Func<T> caseMississippi, Func<T> caseMissouri, Func<T> casePennsylvania, Func<T> caseRhodeIsland, Func<T> caseSouthCarolina, Func<T> caseSouthDakota, Func<T> caseTennessee, Func<T> caseTexas, Func<T> caseUtah, Func<T> caseVermont, Func<T> caseVirginia, Func<T> caseWashington, Func<T> caseWestVirginia, Func<T> caseWisconsin, Func<T> caseWyoming)
    {
        switch (Value)
        {
            case AlabamaValue:
                return caseAlabama();
            case AlaskaValue:
                return caseAlaska();
            case ArizonaValue:
                return caseArizona();
            case ArkansasValue:
                return caseArkansas();
            case CaliforniaValue:
                return caseCalifornia();
            case ColoradoValue:
                return caseColorado();
            case ConnecticutValue:
                return caseConnecticut();
            case DelawareValue:
                return caseDelaware();
            case DistrictOfColumbiaValue:
                return caseDistrictOfColumbia();
            case FloridaValue:
                return caseFlorida();
            case GeorgiaValue:
                return caseGeorgia();
            case HawaiiValue:
                return caseHawaii();
            case IdahoValue:
                return caseIdaho();
            case IllinoisValue:
                return caseIllinois();
            case IndianaValue:
                return caseIndiana();
            case IowaValue:
                return caseIowa();
            case KansasValue:
                return caseKansas();
            case KentuckyValue:
                return caseKentucky();
            case LouisianaValue:
                return caseLouisiana();
            case MaineValue:
                return caseMaine();
            case MontanaValue:
                return caseMontana();
            case NebraskaValue:
                return caseNebraska();
            case NevadaValue:
                return caseNevada();
            case NewHampshireValue:
                return caseNewHampshire();
            case NewJerseyValue:
                return caseNewJersey();
            case NewMexicoValue:
                return caseNewMexico();
            case NewYorkValue:
                return caseNewYork();
            case NorthCarolinaValue:
                return caseNorthCarolina();
            case NorthDakotaValue:
                return caseNorthDakota();
            case OhioValue:
                return caseOhio();
            case OklahomaValue:
                return caseOklahoma();
            case OregonValue:
                return caseOregon();
            case MarylandValue:
                return caseMaryland();
            case MassachusettsValue:
                return caseMassachusetts();
            case MichiganValue:
                return caseMichigan();
            case MinnesotaValue:
                return caseMinnesota();
            case MississippiValue:
                return caseMississippi();
            case MissouriValue:
                return caseMissouri();
            case PennsylvaniaValue:
                return casePennsylvania();
            case RhodeIslandValue:
                return caseRhodeIsland();
            case SouthCarolinaValue:
                return caseSouthCarolina();
            case SouthDakotaValue:
                return caseSouthDakota();
            case TennesseeValue:
                return caseTennessee();
            case TexasValue:
                return caseTexas();
            case UtahValue:
                return caseUtah();
            case VermontValue:
                return caseVermont();
            case VirginiaValue:
                return caseVirginia();
            case WashingtonValue:
                return caseWashington();
            case WestVirginiaValue:
                return caseWestVirginia();
            case WisconsinValue:
                return caseWisconsin();
            case WyomingValue:
                return caseWyoming();
            default:
                throw new InvalidOperationException($"Unhandled state: {Value}");
        }
    }

    /// <summary>
    /// Attempts to match the current enum value against all possible cases and executes the corresponding delegate.
    /// Returns false if no match is found.
    /// </summary>
    /// <param name="caseAlabama">The delegate to execute for the Alabama case.</param>
    /// <param name="caseAlaska">The delegate to execute for the Alaska case.</param>
    /// <param name="caseArizona">The delegate to execute for the Arizona case.</param>
    /// <param name="caseArkansas">The delegate to execute for the Arkansas case.</param>
    /// <param name="caseCalifornia">The delegate to execute for the California case.</param>
    /// <param name="caseColorado">The delegate to execute for the Colorado case.</param>
    /// <param name="caseConnecticut">The delegate to execute for the Connecticut case.</param>
    /// <param name="caseDelaware">The delegate to execute for the Delaware case.</param>
    /// <param name="caseDistrictOfColumbia">The delegate to execute for the DistrictOfColumbia case.</param>
    /// <param name="caseFlorida">The delegate to execute for the Florida case.</param>
    /// <param name="caseGeorgia">The delegate to execute for the Georgia case.</param>
    /// <param name="caseHawaii">The delegate to execute for the Hawaii case.</param>
    /// <param name="caseIdaho">The delegate to execute for the Idaho case.</param>
    /// <param name="caseIllinois">The delegate to execute for the Illinois case.</param>
    /// <param name="caseIndiana">The delegate to execute for the Indiana case.</param>
    /// <param name="caseIowa">The delegate to execute for the Iowa case.</param>
    /// <param name="caseKansas">The delegate to execute for the Kansas case.</param>
    /// <param name="caseKentucky">The delegate to execute for the Kentucky case.</param>
    /// <param name="caseLouisiana">The delegate to execute for the Louisiana case.</param>
    /// <param name="caseMaine">The delegate to execute for the Maine case.</param>
    /// <param name="caseMontana">The delegate to execute for the Montana case.</param>
    /// <param name="caseNebraska">The delegate to execute for the Nebraska case.</param>
    /// <param name="caseNevada">The delegate to execute for the Nevada case.</param>
    /// <param name="caseNewHampshire">The delegate to execute for the NewHampshire case.</param>
    /// <param name="caseNewJersey">The delegate to execute for the NewJersey case.</param>
    /// <param name="caseNewMexico">The delegate to execute for the NewMexico case.</param>
    /// <param name="caseNewYork">The delegate to execute for the NewYork case.</param>
    /// <param name="caseNorthCarolina">The delegate to execute for the NorthCarolina case.</param>
    /// <param name="caseNorthDakota">The delegate to execute for the NorthDakota case.</param>
    /// <param name="caseOhio">The delegate to execute for the Ohio case.</param>
    /// <param name="caseOklahoma">The delegate to execute for the Oklahoma case.</param>
    /// <param name="caseOregon">The delegate to execute for the Oregon case.</param>
    /// <param name="caseMaryland">The delegate to execute for the Maryland case.</param>
    /// <param name="caseMassachusetts">The delegate to execute for the Massachusetts case.</param>
    /// <param name="caseMichigan">The delegate to execute for the Michigan case.</param>
    /// <param name="caseMinnesota">The delegate to execute for the Minnesota case.</param>
    /// <param name="caseMississippi">The delegate to execute for the Mississippi case.</param>
    /// <param name="caseMissouri">The delegate to execute for the Missouri case.</param>
    /// <param name="casePennsylvania">The delegate to execute for the Pennsylvania case.</param>
    /// <param name="caseRhodeIsland">The delegate to execute for the RhodeIsland case.</param>
    /// <param name="caseSouthCarolina">The delegate to execute for the SouthCarolina case.</param>
    /// <param name="caseSouthDakota">The delegate to execute for the SouthDakota case.</param>
    /// <param name="caseTennessee">The delegate to execute for the Tennessee case.</param>
    /// <param name="caseTexas">The delegate to execute for the Texas case.</param>
    /// <param name="caseUtah">The delegate to execute for the Utah case.</param>
    /// <param name="caseVermont">The delegate to execute for the Vermont case.</param>
    /// <param name="caseVirginia">The delegate to execute for the Virginia case.</param>
    /// <param name="caseWashington">The delegate to execute for the Washington case.</param>
    /// <param name="caseWestVirginia">The delegate to execute for the WestVirginia case.</param>
    /// <param name="caseWisconsin">The delegate to execute for the Wisconsin case.</param>
    /// <param name="caseWyoming">The delegate to execute for the Wyoming case.</param>
    /// <returns>True if a match was found and the corresponding delegate was executed, false otherwise.</returns>
    public bool TryMatch(Action caseAlabama, Action caseAlaska, Action caseArizona, Action caseArkansas, Action caseCalifornia, Action caseColorado, Action caseConnecticut, Action caseDelaware, Action caseDistrictOfColumbia, Action caseFlorida, Action caseGeorgia, Action caseHawaii, Action caseIdaho, Action caseIllinois, Action caseIndiana, Action caseIowa, Action caseKansas, Action caseKentucky, Action caseLouisiana, Action caseMaine, Action caseMontana, Action caseNebraska, Action caseNevada, Action caseNewHampshire, Action caseNewJersey, Action caseNewMexico, Action caseNewYork, Action caseNorthCarolina, Action caseNorthDakota, Action caseOhio, Action caseOklahoma, Action caseOregon, Action caseMaryland, Action caseMassachusetts, Action caseMichigan, Action caseMinnesota, Action caseMississippi, Action caseMissouri, Action casePennsylvania, Action caseRhodeIsland, Action caseSouthCarolina, Action caseSouthDakota, Action caseTennessee, Action caseTexas, Action caseUtah, Action caseVermont, Action caseVirginia, Action caseWashington, Action caseWestVirginia, Action caseWisconsin, Action caseWyoming)
    {
        switch (Value)
        {
            case AlabamaValue:
                caseAlabama();
                return true;
            case AlaskaValue:
                caseAlaska();
                return true;
            case ArizonaValue:
                caseArizona();
                return true;
            case ArkansasValue:
                caseArkansas();
                return true;
            case CaliforniaValue:
                caseCalifornia();
                return true;
            case ColoradoValue:
                caseColorado();
                return true;
            case ConnecticutValue:
                caseConnecticut();
                return true;
            case DelawareValue:
                caseDelaware();
                return true;
            case DistrictOfColumbiaValue:
                caseDistrictOfColumbia();
                return true;
            case FloridaValue:
                caseFlorida();
                return true;
            case GeorgiaValue:
                caseGeorgia();
                return true;
            case HawaiiValue:
                caseHawaii();
                return true;
            case IdahoValue:
                caseIdaho();
                return true;
            case IllinoisValue:
                caseIllinois();
                return true;
            case IndianaValue:
                caseIndiana();
                return true;
            case IowaValue:
                caseIowa();
                return true;
            case KansasValue:
                caseKansas();
                return true;
            case KentuckyValue:
                caseKentucky();
                return true;
            case LouisianaValue:
                caseLouisiana();
                return true;
            case MaineValue:
                caseMaine();
                return true;
            case MontanaValue:
                caseMontana();
                return true;
            case NebraskaValue:
                caseNebraska();
                return true;
            case NevadaValue:
                caseNevada();
                return true;
            case NewHampshireValue:
                caseNewHampshire();
                return true;
            case NewJerseyValue:
                caseNewJersey();
                return true;
            case NewMexicoValue:
                caseNewMexico();
                return true;
            case NewYorkValue:
                caseNewYork();
                return true;
            case NorthCarolinaValue:
                caseNorthCarolina();
                return true;
            case NorthDakotaValue:
                caseNorthDakota();
                return true;
            case OhioValue:
                caseOhio();
                return true;
            case OklahomaValue:
                caseOklahoma();
                return true;
            case OregonValue:
                caseOregon();
                return true;
            case MarylandValue:
                caseMaryland();
                return true;
            case MassachusettsValue:
                caseMassachusetts();
                return true;
            case MichiganValue:
                caseMichigan();
                return true;
            case MinnesotaValue:
                caseMinnesota();
                return true;
            case MississippiValue:
                caseMississippi();
                return true;
            case MissouriValue:
                caseMissouri();
                return true;
            case PennsylvaniaValue:
                casePennsylvania();
                return true;
            case RhodeIslandValue:
                caseRhodeIsland();
                return true;
            case SouthCarolinaValue:
                caseSouthCarolina();
                return true;
            case SouthDakotaValue:
                caseSouthDakota();
                return true;
            case TennesseeValue:
                caseTennessee();
                return true;
            case TexasValue:
                caseTexas();
                return true;
            case UtahValue:
                caseUtah();
                return true;
            case VermontValue:
                caseVermont();
                return true;
            case VirginiaValue:
                caseVirginia();
                return true;
            case WashingtonValue:
                caseWashington();
                return true;
            case WestVirginiaValue:
                caseWestVirginia();
                return true;
            case WisconsinValue:
                caseWisconsin();
                return true;
            case WyomingValue:
                caseWyoming();
                return true;
            default:
                return false;
        }
    }

    /// <summary>
    /// Attempts to match the current enum value against all possible cases and returns the result of executing the corresponding delegate.
    /// Returns false if no match is found.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <param name="caseAlabama">The delegate to execute for the Alabama case.</param>
    /// <param name="caseAlaska">The delegate to execute for the Alaska case.</param>
    /// <param name="caseArizona">The delegate to execute for the Arizona case.</param>
    /// <param name="caseArkansas">The delegate to execute for the Arkansas case.</param>
    /// <param name="caseCalifornia">The delegate to execute for the California case.</param>
    /// <param name="caseColorado">The delegate to execute for the Colorado case.</param>
    /// <param name="caseConnecticut">The delegate to execute for the Connecticut case.</param>
    /// <param name="caseDelaware">The delegate to execute for the Delaware case.</param>
    /// <param name="caseDistrictOfColumbia">The delegate to execute for the DistrictOfColumbia case.</param>
    /// <param name="caseFlorida">The delegate to execute for the Florida case.</param>
    /// <param name="caseGeorgia">The delegate to execute for the Georgia case.</param>
    /// <param name="caseHawaii">The delegate to execute for the Hawaii case.</param>
    /// <param name="caseIdaho">The delegate to execute for the Idaho case.</param>
    /// <param name="caseIllinois">The delegate to execute for the Illinois case.</param>
    /// <param name="caseIndiana">The delegate to execute for the Indiana case.</param>
    /// <param name="caseIowa">The delegate to execute for the Iowa case.</param>
    /// <param name="caseKansas">The delegate to execute for the Kansas case.</param>
    /// <param name="caseKentucky">The delegate to execute for the Kentucky case.</param>
    /// <param name="caseLouisiana">The delegate to execute for the Louisiana case.</param>
    /// <param name="caseMaine">The delegate to execute for the Maine case.</param>
    /// <param name="caseMontana">The delegate to execute for the Montana case.</param>
    /// <param name="caseNebraska">The delegate to execute for the Nebraska case.</param>
    /// <param name="caseNevada">The delegate to execute for the Nevada case.</param>
    /// <param name="caseNewHampshire">The delegate to execute for the NewHampshire case.</param>
    /// <param name="caseNewJersey">The delegate to execute for the NewJersey case.</param>
    /// <param name="caseNewMexico">The delegate to execute for the NewMexico case.</param>
    /// <param name="caseNewYork">The delegate to execute for the NewYork case.</param>
    /// <param name="caseNorthCarolina">The delegate to execute for the NorthCarolina case.</param>
    /// <param name="caseNorthDakota">The delegate to execute for the NorthDakota case.</param>
    /// <param name="caseOhio">The delegate to execute for the Ohio case.</param>
    /// <param name="caseOklahoma">The delegate to execute for the Oklahoma case.</param>
    /// <param name="caseOregon">The delegate to execute for the Oregon case.</param>
    /// <param name="caseMaryland">The delegate to execute for the Maryland case.</param>
    /// <param name="caseMassachusetts">The delegate to execute for the Massachusetts case.</param>
    /// <param name="caseMichigan">The delegate to execute for the Michigan case.</param>
    /// <param name="caseMinnesota">The delegate to execute for the Minnesota case.</param>
    /// <param name="caseMississippi">The delegate to execute for the Mississippi case.</param>
    /// <param name="caseMissouri">The delegate to execute for the Missouri case.</param>
    /// <param name="casePennsylvania">The delegate to execute for the Pennsylvania case.</param>
    /// <param name="caseRhodeIsland">The delegate to execute for the RhodeIsland case.</param>
    /// <param name="caseSouthCarolina">The delegate to execute for the SouthCarolina case.</param>
    /// <param name="caseSouthDakota">The delegate to execute for the SouthDakota case.</param>
    /// <param name="caseTennessee">The delegate to execute for the Tennessee case.</param>
    /// <param name="caseTexas">The delegate to execute for the Texas case.</param>
    /// <param name="caseUtah">The delegate to execute for the Utah case.</param>
    /// <param name="caseVermont">The delegate to execute for the Vermont case.</param>
    /// <param name="caseVirginia">The delegate to execute for the Virginia case.</param>
    /// <param name="caseWashington">The delegate to execute for the Washington case.</param>
    /// <param name="caseWestVirginia">The delegate to execute for the WestVirginia case.</param>
    /// <param name="caseWisconsin">The delegate to execute for the Wisconsin case.</param>
    /// <param name="caseWyoming">The delegate to execute for the Wyoming case.</param>
    /// <param name="result">The result of executing the matching delegate, if a match was found.</param>
    /// <returns>True if a match was found and the corresponding delegate was executed, false otherwise.</returns>
    public bool TryMatch<T>(Func<T> caseAlabama, Func<T> caseAlaska, Func<T> caseArizona, Func<T> caseArkansas, Func<T> caseCalifornia, Func<T> caseColorado, Func<T> caseConnecticut, Func<T> caseDelaware, Func<T> caseDistrictOfColumbia, Func<T> caseFlorida, Func<T> caseGeorgia, Func<T> caseHawaii, Func<T> caseIdaho, Func<T> caseIllinois, Func<T> caseIndiana, Func<T> caseIowa, Func<T> caseKansas, Func<T> caseKentucky, Func<T> caseLouisiana, Func<T> caseMaine, Func<T> caseMontana, Func<T> caseNebraska, Func<T> caseNevada, Func<T> caseNewHampshire, Func<T> caseNewJersey, Func<T> caseNewMexico, Func<T> caseNewYork, Func<T> caseNorthCarolina, Func<T> caseNorthDakota, Func<T> caseOhio, Func<T> caseOklahoma, Func<T> caseOregon, Func<T> caseMaryland, Func<T> caseMassachusetts, Func<T> caseMichigan, Func<T> caseMinnesota, Func<T> caseMississippi, Func<T> caseMissouri, Func<T> casePennsylvania, Func<T> caseRhodeIsland, Func<T> caseSouthCarolina, Func<T> caseSouthDakota, Func<T> caseTennessee, Func<T> caseTexas, Func<T> caseUtah, Func<T> caseVermont, Func<T> caseVirginia, Func<T> caseWashington, Func<T> caseWestVirginia, Func<T> caseWisconsin, Func<T> caseWyoming, out T result)
    {
        switch (Value)
        {
            case AlabamaValue:
                result = caseAlabama();
                return true;
            case AlaskaValue:
                result = caseAlaska();
                return true;
            case ArizonaValue:
                result = caseArizona();
                return true;
            case ArkansasValue:
                result = caseArkansas();
                return true;
            case CaliforniaValue:
                result = caseCalifornia();
                return true;
            case ColoradoValue:
                result = caseColorado();
                return true;
            case ConnecticutValue:
                result = caseConnecticut();
                return true;
            case DelawareValue:
                result = caseDelaware();
                return true;
            case DistrictOfColumbiaValue:
                result = caseDistrictOfColumbia();
                return true;
            case FloridaValue:
                result = caseFlorida();
                return true;
            case GeorgiaValue:
                result = caseGeorgia();
                return true;
            case HawaiiValue:
                result = caseHawaii();
                return true;
            case IdahoValue:
                result = caseIdaho();
                return true;
            case IllinoisValue:
                result = caseIllinois();
                return true;
            case IndianaValue:
                result = caseIndiana();
                return true;
            case IowaValue:
                result = caseIowa();
                return true;
            case KansasValue:
                result = caseKansas();
                return true;
            case KentuckyValue:
                result = caseKentucky();
                return true;
            case LouisianaValue:
                result = caseLouisiana();
                return true;
            case MaineValue:
                result = caseMaine();
                return true;
            case MontanaValue:
                result = caseMontana();
                return true;
            case NebraskaValue:
                result = caseNebraska();
                return true;
            case NevadaValue:
                result = caseNevada();
                return true;
            case NewHampshireValue:
                result = caseNewHampshire();
                return true;
            case NewJerseyValue:
                result = caseNewJersey();
                return true;
            case NewMexicoValue:
                result = caseNewMexico();
                return true;
            case NewYorkValue:
                result = caseNewYork();
                return true;
            case NorthCarolinaValue:
                result = caseNorthCarolina();
                return true;
            case NorthDakotaValue:
                result = caseNorthDakota();
                return true;
            case OhioValue:
                result = caseOhio();
                return true;
            case OklahomaValue:
                result = caseOklahoma();
                return true;
            case OregonValue:
                result = caseOregon();
                return true;
            case MarylandValue:
                result = caseMaryland();
                return true;
            case MassachusettsValue:
                result = caseMassachusetts();
                return true;
            case MichiganValue:
                result = caseMichigan();
                return true;
            case MinnesotaValue:
                result = caseMinnesota();
                return true;
            case MississippiValue:
                result = caseMississippi();
                return true;
            case MissouriValue:
                result = caseMissouri();
                return true;
            case PennsylvaniaValue:
                result = casePennsylvania();
                return true;
            case RhodeIslandValue:
                result = caseRhodeIsland();
                return true;
            case SouthCarolinaValue:
                result = caseSouthCarolina();
                return true;
            case SouthDakotaValue:
                result = caseSouthDakota();
                return true;
            case TennesseeValue:
                result = caseTennessee();
                return true;
            case TexasValue:
                result = caseTexas();
                return true;
            case UtahValue:
                result = caseUtah();
                return true;
            case VermontValue:
                result = caseVermont();
                return true;
            case VirginiaValue:
                result = caseVirginia();
                return true;
            case WashingtonValue:
                result = caseWashington();
                return true;
            case WestVirginiaValue:
                result = caseWestVirginia();
                return true;
            case WisconsinValue:
                result = caseWisconsin();
                return true;
            case WyomingValue:
                result = caseWyoming();
                return true;
            default:
                result = default!;
                return false;
        }
    }

    public static UsaState? TryParse(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return value switch
        {
            _ when string.Equals(value, AlabamaValue, StringComparison.OrdinalIgnoreCase) => Alabama,

            _ when string.Equals(value, AlaskaValue, StringComparison.OrdinalIgnoreCase) => Alaska,

            _ when string.Equals(value, ArizonaValue, StringComparison.OrdinalIgnoreCase) => Arizona,

            _ when string.Equals(value, ArkansasValue, StringComparison.OrdinalIgnoreCase) => Arkansas,

            _ when string.Equals(value, CaliforniaValue, StringComparison.OrdinalIgnoreCase) => California,

            _ when string.Equals(value, ColoradoValue, StringComparison.OrdinalIgnoreCase) => Colorado,

            _ when string.Equals(value, ConnecticutValue, StringComparison.OrdinalIgnoreCase) => Connecticut,

            _ when string.Equals(value, DelawareValue, StringComparison.OrdinalIgnoreCase) => Delaware,

            _ when string.Equals(value, DistrictOfColumbiaValue, StringComparison.OrdinalIgnoreCase) => DistrictOfColumbia,

            _ when string.Equals(value, FloridaValue, StringComparison.OrdinalIgnoreCase) => Florida,

            _ when string.Equals(value, GeorgiaValue, StringComparison.OrdinalIgnoreCase) => Georgia,

            _ when string.Equals(value, HawaiiValue, StringComparison.OrdinalIgnoreCase) => Hawaii,

            _ when string.Equals(value, IdahoValue, StringComparison.OrdinalIgnoreCase) => Idaho,

            _ when string.Equals(value, IllinoisValue, StringComparison.OrdinalIgnoreCase) => Illinois,

            _ when string.Equals(value, IndianaValue, StringComparison.OrdinalIgnoreCase) => Indiana,

            _ when string.Equals(value, IowaValue, StringComparison.OrdinalIgnoreCase) => Iowa,

            _ when string.Equals(value, KansasValue, StringComparison.OrdinalIgnoreCase) => Kansas,

            _ when string.Equals(value, KentuckyValue, StringComparison.OrdinalIgnoreCase) => Kentucky,

            _ when string.Equals(value, LouisianaValue, StringComparison.OrdinalIgnoreCase) => Louisiana,

            _ when string.Equals(value, MaineValue, StringComparison.OrdinalIgnoreCase) => Maine,

            _ when string.Equals(value, MontanaValue, StringComparison.OrdinalIgnoreCase) => Montana,

            _ when string.Equals(value, NebraskaValue, StringComparison.OrdinalIgnoreCase) => Nebraska,

            _ when string.Equals(value, NevadaValue, StringComparison.OrdinalIgnoreCase) => Nevada,

            _ when string.Equals(value, NewHampshireValue, StringComparison.OrdinalIgnoreCase) => NewHampshire,

            _ when string.Equals(value, NewJerseyValue, StringComparison.OrdinalIgnoreCase) => NewJersey,

            _ when string.Equals(value, NewMexicoValue, StringComparison.OrdinalIgnoreCase) => NewMexico,

            _ when string.Equals(value, NewYorkValue, StringComparison.OrdinalIgnoreCase) => NewYork,

            _ when string.Equals(value, NorthCarolinaValue, StringComparison.OrdinalIgnoreCase) => NorthCarolina,

            _ when string.Equals(value, NorthDakotaValue, StringComparison.OrdinalIgnoreCase) => NorthDakota,

            _ when string.Equals(value, OhioValue, StringComparison.OrdinalIgnoreCase) => Ohio,

            _ when string.Equals(value, OklahomaValue, StringComparison.OrdinalIgnoreCase) => Oklahoma,

            _ when string.Equals(value, OregonValue, StringComparison.OrdinalIgnoreCase) => Oregon,

            _ when string.Equals(value, MarylandValue, StringComparison.OrdinalIgnoreCase) => Maryland,

            _ when string.Equals(value, MassachusettsValue, StringComparison.OrdinalIgnoreCase) => Massachusetts,

            _ when string.Equals(value, MichiganValue, StringComparison.OrdinalIgnoreCase) => Michigan,

            _ when string.Equals(value, MinnesotaValue, StringComparison.OrdinalIgnoreCase) => Minnesota,

            _ when string.Equals(value, MississippiValue, StringComparison.OrdinalIgnoreCase) => Mississippi,

            _ when string.Equals(value, MissouriValue, StringComparison.OrdinalIgnoreCase) => Missouri,

            _ when string.Equals(value, PennsylvaniaValue, StringComparison.OrdinalIgnoreCase) => Pennsylvania,

            _ when string.Equals(value, RhodeIslandValue, StringComparison.OrdinalIgnoreCase) => RhodeIsland,

            _ when string.Equals(value, SouthCarolinaValue, StringComparison.OrdinalIgnoreCase) => SouthCarolina,

            _ when string.Equals(value, SouthDakotaValue, StringComparison.OrdinalIgnoreCase) => SouthDakota,

            _ when string.Equals(value, TennesseeValue, StringComparison.OrdinalIgnoreCase) => Tennessee,

            _ when string.Equals(value, TexasValue, StringComparison.OrdinalIgnoreCase) => Texas,

            _ when string.Equals(value, UtahValue, StringComparison.OrdinalIgnoreCase) => Utah,

            _ when string.Equals(value, VermontValue, StringComparison.OrdinalIgnoreCase) => Vermont,

            _ when string.Equals(value, VirginiaValue, StringComparison.OrdinalIgnoreCase) => Virginia,

            _ when string.Equals(value, WashingtonValue, StringComparison.OrdinalIgnoreCase) => Washington,

            _ when string.Equals(value, WestVirginiaValue, StringComparison.OrdinalIgnoreCase) => WestVirginia,

            _ when string.Equals(value, WisconsinValue, StringComparison.OrdinalIgnoreCase) => Wisconsin,

            _ when string.Equals(value, WyomingValue, StringComparison.OrdinalIgnoreCase) => Wyoming,
            _ => null,
        };
    }

    public static bool TryParse(string? value, out UsaState parsed) => TryParse(value, null, out parsed);

    public static UsaState Parse(string value) => Parse(value, null);

    public static UsaState Parse(string s, IFormatProvider? provider)
    {
        Guard.IsNotNull(s);

        if (TryParse(s, provider, out UsaState parsed))
        {
            return parsed;
        }
        else
        {
            throw new ArgumentOutOfRangeException($"The value {s} is not a valid UsaState.");
        }
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out UsaState result)
    {
        UsaState? parsed = TryParse(s);
        if (parsed.HasValue)
        {
            result = parsed.Value;
            return true;
        }

        result = default;
        return false;
    }

    public class UsaStateJsonConverter : JsonConverter<UsaState>
    {
        public override UsaState Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var s = reader.GetString();

            if (!string.IsNullOrEmpty(s) && UsaState.TryParse(s, out UsaState result))
            {
                return result;
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, UsaState value, JsonSerializerOptions options) =>
            writer.WriteStringValue(value.Value);

        public override void WriteAsPropertyName(Utf8JsonWriter writer, UsaState value, JsonSerializerOptions options) =>
            writer.WritePropertyName(value.Value);

        public override UsaState ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return Read(ref reader, typeToConvert, options);
        }
    }

    // TypeConverter for UsaState to and from string
    public class UsaStateTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) =>
            sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

        public override bool CanConvertTo(ITypeDescriptorContext? context, [NotNullWhen(true)] Type? destinationType) =>
            destinationType == typeof(string) || base.CanConvertTo(context, destinationType);

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if (value is string s)
            {
                return UsaState.TryParse(s) ?? default;
            }

            return base.ConvertFrom(context, culture, value) ?? default;
        }

        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            if (value is UsaState type && destinationType == typeof(string))
            {
                return type.ToString();
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
