using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "UIInstaller", menuName = "Installers/UIInstaller")]
public class UIInstaller : ScriptableObjectInstaller<UIInstaller>
{
    [SerializeField] private ForkliftUIController forkliftUIController;

    public override void InstallBindings()
    {
        Container.Bind<ForkliftUIController>().FromInstance(forkliftUIController).AsSingle();
    }
}