/*
*  FILE         : OutlineText.cs
*  PROJECT      : PROG3220-Systems Project
*  PROGRAMMER   : Bence Karner
*  DESCRIPTION  : Sole responsibility is to contain the OutlineText class
*  REFERENCES   : The following code was taken from an online tutorial, and altered to suit the needs of the project.
*   Сергей Дроба[Username]. (Feb 12, 2017). Unity UI Text Perfect outline. Retrieved March 4, 2020, from https://www.youtube.com/watch?v=yXDEH3EcaAg
*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The class is used to create an outline effect on the UI text element attached to the script.
/// It uses the base UI shadow class to create the outline in all directions around the text.
/// </summary>
public class OutlineText : Shadow
{

    #region Properties
    /// <summary>
    /// 
    /// </summary>
    [Range(0, 15)]
    public float m_size = 3.0f;

    /// <summary>
    /// 
    /// </summary>
    public bool glintEffect;

    /// <summary>
    /// 
    /// </summary>
    [RangeAttribute(0, 5)]
    public int glintVertex = 0;

    /// <summary>
    /// 
    /// </summary>
    [RangeAttribute(0, 3)]
    public int glintWidth = 0;

    /// <summary>
    /// 
    /// </summary>
    public Color glintColor;
    #endregion


    /// <summary>
    /// 
    /// </summary>
    /// <param name="vertexHelper"></param>
    public override void ModifyMesh(VertexHelper vertexHelper)
    {
        if (!IsActive())
        {
            return;
        }

        var verts = new List<UIVertex>();
        vertexHelper.GetUIVertexStream(verts);


        var neededCpacity = verts.Count * 5;
        if (verts.Capacity < neededCpacity)
        {
            verts.Capacity = neededCpacity;
        }


        if (glintEffect)
        {
            for (int i = 0; i < verts.Count; i++)
            {
                UIVertex item = verts[i];

                for (int j = -glintWidth; j <= glintWidth; j++)
                {

                    if (i % 6 == Mathf.Repeat(glintVertex + j, 6))
                        item.color = glintColor;
                }

                verts[i] = item;
            }
        }


        Vector2 m_effectDistance = new Vector2(m_size, m_size);
        var start = 0;
        var end = verts.Count;
        ApplyShadowZeroAlloc(verts, effectColor, start, verts.Count, m_effectDistance.x, m_effectDistance.y);

        start = end;
        end = verts.Count;
        ApplyShadowZeroAlloc(verts, effectColor, start, verts.Count, m_effectDistance.x, -m_effectDistance.y);

        start = end;
        end = verts.Count;
        ApplyShadowZeroAlloc(verts, effectColor, start, verts.Count, -m_effectDistance.x, m_effectDistance.y);

        start = end;
        end = verts.Count;
        ApplyShadowZeroAlloc(verts, effectColor, start, verts.Count, -m_effectDistance.x, -m_effectDistance.y);

        //////////////////////////////
        start = end;
        end = verts.Count;
        ApplyShadowZeroAlloc(verts, effectColor, start, verts.Count, 0, m_effectDistance.y * 1.5f);

        start = end;
        end = verts.Count;
        ApplyShadowZeroAlloc(verts, effectColor, start, verts.Count, m_effectDistance.x * 1.5f, 0);

        start = end;
        end = verts.Count;
        ApplyShadowZeroAlloc(verts, effectColor, start, verts.Count, -m_effectDistance.x * 1.5f, 0);

        start = end;
        end = verts.Count;
        ApplyShadowZeroAlloc(verts, effectColor, start, verts.Count, 0, -m_effectDistance.y * 1.5f);

        vertexHelper.Clear();
        vertexHelper.AddUIVertexTriangleStream(verts);
    }

}//class