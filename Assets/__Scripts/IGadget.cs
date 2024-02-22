using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGadget 
{
    bool GadgetUse(DrayScript tDray, System.Func<IGadget, bool> tDoneCallBack);
    bool GadgetCancel();
    string name { get; }
}
