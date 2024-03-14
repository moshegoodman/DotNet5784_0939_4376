using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PL;
internal class EngineerExperience : IEnumerable
{
    static readonly IEnumerable<BO.EngineerExperience> s_enums =
(Enum.GetValues(typeof(BO.EngineerExperience)) as IEnumerable<BO.EngineerExperience>)!;

    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();

}
internal class GeneralFilter : IEnumerable
{
    static readonly IEnumerable<BO.General> s_enums =
 (Enum.GetValues(typeof(BO.General)) as IEnumerable<BO.General>)!;

    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();

}

internal class EngineerExperienceToUpdate : IEnumerable
{
    static readonly IEnumerable<BO.EngineerExperience> s_enums =
(Enum.GetValues(typeof(BO.EngineerExperience)) as IEnumerable<BO.EngineerExperience>)!.Where(experience => experience != BO.EngineerExperience.None);

    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();

}
