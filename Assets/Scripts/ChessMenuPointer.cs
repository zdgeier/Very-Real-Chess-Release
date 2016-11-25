using UnityEngine;
using NewtonVR;
using UnityEngine.UI;
using System.Collections;

public class ChessMenuPointer : MonoBehaviour {

    public Color LineColor;
    public float LineWidth = 0.02f;
    public LayerMask mask;

    private LineRenderer Line;

    private NVRHand Hand;

    private void Awake ()
    {
        Line = this.GetComponent<LineRenderer>();
        Hand = this.GetComponent<NVRHand>();

        if (Line == null)
        {
            Line = this.gameObject.AddComponent<LineRenderer>();
        }

        if (Line.sharedMaterial == null)
        {
            Line.material = new Material(Shader.Find("Unlit/Color"));
            Line.material.SetColor("_Color", LineColor);
            Line.SetColors(LineColor, LineColor);
        }

        Line.useWorldSpace = true;
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
        Line.enabled = (Hand != null && Hand.Inputs[Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger].SingleAxis > 0.01f);

        if (Line.enabled == true)
        {
            Line.material.SetColor("_Color", LineColor);
            Line.SetColors(LineColor, LineColor);
            Line.SetWidth(LineWidth, LineWidth);

            RaycastHit hitInfo;

            bool hit = Physics.Raycast(this.transform.position, this.transform.forward, out hitInfo, 1000, mask);
            Vector3 endPoint;

            if (hit == true)
            {
                endPoint = hitInfo.point;

                if (Hand.Inputs[Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger].PressDown == true)
                {
                    Button button = hitInfo.collider.transform.parent.GetComponent<Button>();

                    if (button != null)
                    {
                        if (button.transform.Find("DifficultyText") != null)
                        {
                            this.transform.parent.GetComponent<ChessMenu>().ResetDifficultyColors();
                            button.transform.Find("DifficultyText").gameObject.GetComponent<Text>().color = Color.green;
                        }
                        else if (button.transform.Find("SideText") != null)
                        {
                            this.transform.parent.GetComponent<ChessMenu>().ResetSideColors();
                            button.transform.Find("SideText").gameObject.GetComponent<Text>().color = Color.green;
                        }
                            
                        button.onClick.Invoke();
                    }
                }
            }
            else
            {
                endPoint = this.transform.position + (this.transform.forward * 1000f);
            }

            Line.SetPositions(new Vector3[] { this.transform.position, endPoint });
        }
    }
}