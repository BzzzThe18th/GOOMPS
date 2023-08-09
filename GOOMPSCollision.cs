using UnityEngine;

namespace GOOMPS
{
    class GOOMPSCollision : MonoBehaviour
    {
        public static void ChangePlayerVisibility(VRRig _rig, bool _visible)
        {
            if (Cfg.audio.Value) GorillaTagger.Instance?.offlineVRRig?.tagSound.PlayOneShot(GorillaTagger.Instance.offlineVRRig.clipToPlay[5]);
            _rig.mainSkin.enabled = _visible;
            _rig.muted = Cfg.mute.Value ? !_visible : false;
            _rig.transform.Find("rig/body/gorillachest").gameObject.SetActive(_visible);
            _rig.transform.Find("rig/body/head/gorillaface").gameObject.SetActive(_visible);
        }

        void OnTriggerEnter(Collider coll)
        {
            if (coll.name.Contains("Body"))
            {
                ChangePlayerVisibility(coll.transform.parent.parent.parent.GetComponent<VRRig>(), false);
            }
        }

        void OnTriggerExit(Collider coll)
        {
            if (coll.name.Contains("Body"))
            {
                ChangePlayerVisibility(coll.transform.parent.parent.parent.GetComponent<VRRig>(), true);
            }
        }
    }
}