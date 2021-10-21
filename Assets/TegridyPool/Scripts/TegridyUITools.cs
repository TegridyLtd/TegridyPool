/////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2021 Tegridy Ltd                                          //
// Author: Darren Braviner                                                 //
// Contact: db@tegridygames.co.uk                                          //
/////////////////////////////////////////////////////////////////////////////
//                                                                         //
// This program is free software; you can redistribute it and/or modify    //
// it under the terms of the GNU General Public License as published by    //
// the Free Software Foundation; either version 2 of the License, or       //
// (at your option) any later version.                                     //
//                                                                         //
// This program is distributed in the hope that it will be useful,         //
// but WITHOUT ANY WARRANTY.                                               //
//                                                                         //
/////////////////////////////////////////////////////////////////////////////
//                                                                         //
// You should have received a copy of the GNU General Public License       //
// along with this program; if not, write to the Free Software             //
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston,              //
// MA 02110-1301 USA                                                       //
//                                                                         //
/////////////////////////////////////////////////////////////////////////////

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace Tegridy.UITools
{
    public class TegridyUITools : MonoBehaviour
    {
        public static GameObject[] DrawStraight(GameObject prefab, int quantity, float spacing, bool vertical, bool add, RectTransform holder)
        {
            //get the position of our first button
            float posY = prefab.GetComponent<RectTransform>().localPosition.y;
            float posX = prefab.GetComponent<RectTransform>().localPosition.x;
            float posZ = prefab.GetComponent<RectTransform>().localPosition.z;
            prefab.SetActive(false);

            GameObject[] array = new GameObject[quantity];

            //create the buttons
            for (int i = 0; i < quantity; i++)
            {
                //create a new buttons and set it active
                GameObject grades = Instantiate(prefab.gameObject);
                grades.SetActive(true);
                grades.transform.SetParent(prefab.transform.parent);

                array[i] = grades;

                //set position of the new button
                grades.GetComponent<RectTransform>().localPosition = prefab.GetComponent<RectTransform>().localPosition;
                grades.GetComponent<RectTransform>().localPosition = new Vector3(posX, posY, posZ);
                grades.GetComponent<RectTransform>().localScale = prefab.GetComponent<RectTransform>().localScale;

                //get the position ready for the next button
                if (vertical)
                {
                    if (add) posY += spacing;
                    else posY -= spacing;
                }
                else
                {
                    if (add) posX += spacing;
                    else posX -= spacing;
                }
            }

            if (holder != null)
            {
                if (vertical) holder.sizeDelta = new Vector2(holder.sizeDelta.x, Mathf.Abs((spacing * quantity) + (spacing / 2)));
                else holder.sizeDelta = new Vector2(Mathf.Abs((spacing * quantity) + (spacing / 2)), holder.sizeDelta.y);
            }
            return array;
        }
        public static GameObject[] DrawTiled(GameObject prefab, int quantity, float spacingH, float spacingV, int colums)
        {
            //get the position of our first button
            float posY = prefab.GetComponent<RectTransform>().localPosition.y;
            float posX = prefab.GetComponent<RectTransform>().localPosition.x;
            float posZ = prefab.GetComponent<RectTransform>().localPosition.z;
            prefab.SetActive(false);

            GameObject[] array = new GameObject[quantity];
            int tile = 0;
            //create the buttons
            for (int i = 0; i < quantity; i++)
            {
                //create a new buttons and set it active
                GameObject grades = Instantiate(prefab.gameObject);
                grades.SetActive(true);
                grades.transform.SetParent(prefab.transform.parent);

                array[i] = grades;

                //set position of the new button
                grades.GetComponent<RectTransform>().localPosition = prefab.GetComponent<RectTransform>().localPosition;
                grades.GetComponent<RectTransform>().localPosition = new Vector3(posX, posY, posZ);
                grades.GetComponent<RectTransform>().localScale = prefab.GetComponent<RectTransform>().localScale;

                //set position for next item
                tile++;
                if (tile % colums == 0 && i != 0)
                {
                    posY += spacingV;
                    posX = prefab.GetComponent<RectTransform>().localPosition.x;
                }
                else posX += spacingH;

            }
            return array;
        }
        public static void ToggleButtons(bool listening, GameObject[] buttons)
        {
            foreach (GameObject but in buttons)
            {
                but.GetComponent<Button>().interactable = listening;
            }
        }
        public static void DestoryOld(GameObject[] old)
        {
            if (old != null)
            {
                foreach (GameObject thisOne in old)
                {
                    Destroy(thisOne);
                }
            }
        }

        public static void SetButtonText(Button button, string text)
        {
            if (button.GetComponentInChildren<TextMeshProUGUI>() != null)
                button.GetComponentInChildren<TextMeshProUGUI>().text = text;
        }

        public static void SetChildText(GameObject parent, string text)
        {
            if (parent.GetComponentInChildren<TextMeshProUGUI>() != null)
                parent.GetComponentInChildren<TextMeshProUGUI>().text = text;
        }


        public static void SetText(TextMeshProUGUI tmpro, string text)
        {
            if (tmpro != null) tmpro.text = text;
        }
        public static IEnumerator ZoomUI(bool open, float speed, RectTransform uiHolder)
        {
            //figure out what to do
            Vector3 target;
            if (open)
            {
                target = new Vector3(0, 0, 0);
                uiHolder.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                target = new Vector3(1, 1, 1);
                uiHolder.localScale = new Vector3(0, 0, 0);
            }

            //scale the ui
            while (target != uiHolder.localScale)
            {
                uiHolder.localScale = Vector3.Lerp(uiHolder.localScale, target, 0.01f);
                yield return new WaitForSeconds(speed);
                Debug.Log("Zoomed a little bit");
            }
        }
    }
}