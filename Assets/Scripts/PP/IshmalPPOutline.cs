using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[UnityEngine.Rendering.PostProcessing.PostProcess(typeof(IshmalPPOutlineRenderer), PostProcessEvent.AfterStack, "Outline")]
public sealed class IshmalPPOutline : PostProcessEffectSettings
{
    public FloatParameter thickneses = new FloatParameter { value = 1f };
    public FloatParameter depthMin = new FloatParameter { value = 0f };
    public FloatParameter depthMax = new FloatParameter { value = 1f };
}
