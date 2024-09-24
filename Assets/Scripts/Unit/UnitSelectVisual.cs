using UnityEngine;

public class UnitSelectVisual : MonoBehaviour
{
    private MeshRenderer m_MeshRenderer;

    private Unit unit;

    private void Instance_OnSelectedUnitChangedEvent()
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (UnitActionSystem.Instance.selectedUnit != null && unit == UnitActionSystem.Instance.selectedUnit)
            m_MeshRenderer.enabled = true;
        else
            m_MeshRenderer.enabled = false;
    }

    private void Awake()
    {
        m_MeshRenderer = GetComponent<MeshRenderer>();

        unit = GetComponentInParent<Unit>();
    }

    private void OnEnable()
    {
        UpdateVisual();

        UnitActionSystem.Instance.OnSelectedUnitChangedEvent += Instance_OnSelectedUnitChangedEvent;
    }

    private void OnDisable()
    {
        UnitActionSystem.Instance.OnSelectedUnitChangedEvent -= Instance_OnSelectedUnitChangedEvent;
    }


}
