using TMPro;
using UnityEngine;
using Zenject;

namespace UI
{
    public class ForkliftUIView : MonoBehaviour
    {
        [SerializeField] private TMP_Text speedText;
        [SerializeField] private TMP_Text forkPositionText;
        [SerializeField] private TMP_Text horizontalInputText;
        [SerializeField] private TMP_Text verticalInputText;
        [SerializeField] private TMP_Text isObjectOnForkText;
        
        private ForkliftUIController forkliftUIController;
        
        [Inject]
        private void Init(ForkliftUIController _forkliftUIController)
        {
            forkliftUIController = _forkliftUIController;
        }
        
        private void Update()
        {
            speedText.text = forkliftUIController.Speed.ToString("F2");
            forkPositionText.text = forkliftUIController.ForkPosition.ToString("F2");
            horizontalInputText.text = forkliftUIController.HorizontalInput.ToString("F2");
            verticalInputText.text = forkliftUIController.HorizontalInput.ToString("F2");
            isObjectOnForkText.text = forkliftUIController.IsObjectOnFork ? "Yes" : "No";
        }
    }
}