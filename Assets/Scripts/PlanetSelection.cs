using System.Collections.Generic;
using UnityEngine;

public class PlanetSelection : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private RectTransform selectionBox;

    private readonly List<Planet> _selectedPlanets = new List<Planet>();
    private Vector2 _startPos;

    public void Clear()
    {
        _selectedPlanets.Clear();
    }

    private void TrySelectPlanet(Vector3 pos)
    {
        var ray = mainCamera.ScreenPointToRay(pos);

        if (Physics.Raycast(ray, out var raycastHit))
        {
            var planet = raycastHit.collider.GetComponent<Planet>();

            if (planet && !planet.Selected && planet.Ruler != null &&
                planet.Ruler.Id == GameManager.Instance.FirstPlayer.Id)
            {
                planet.Select();
                _selectedPlanets.Add(planet);
            }
            else if (planet)
            {
                foreach (var selectedPlanet in _selectedPlanets)
                {
                    selectedPlanet.LaunchSpaceshipsToAnotherPlanet(planet);
                    selectedPlanet.UnSelect();
                }

                _selectedPlanets.Clear();
            }
        }
        else
        {
            foreach (var selectedPlanet in _selectedPlanets)
            {
                selectedPlanet.UnSelect();
            }

            _selectedPlanets.Clear();
        }
    }

    private void Update()
    {
        // if (EventSystem.current.IsPointerOverGameObject())
        //     return;

        if (Input.GetMouseButtonDown(0))
        {
            TrySelectPlanet(Input.mousePosition);
            _startPos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            ReleaseSelectionBox();
        }

        if (Input.GetMouseButton(0))
        {
            UpdateSelectionBox(Input.mousePosition);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            GameManager.Instance.StartNewGame();
        }
    }

    private void UpdateSelectionBox(Vector2 curMousePos)
    {
        if (!selectionBox.gameObject.activeInHierarchy)
            selectionBox.gameObject.SetActive(true);

        var width = curMousePos.x - _startPos.x;
        var height = curMousePos.y - _startPos.y;

        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
        selectionBox.anchoredPosition = _startPos + new Vector2(width / 2, height / 2);
    }

    private void ReleaseSelectionBox()
    {
        selectionBox.gameObject.SetActive(false);

        var min = selectionBox.anchoredPosition - (selectionBox.sizeDelta / 2);
        var max = selectionBox.anchoredPosition + (selectionBox.sizeDelta / 2);

        foreach (var planet in GameManager.Instance.FirstPlayer.Planets)
        {
            var screenPos = mainCamera.WorldToScreenPoint(planet.transform.position);

            if (screenPos.x > min.x && screenPos.x < max.x && screenPos.y > min.y && screenPos.y < max.y)
            {
                _selectedPlanets.Add(planet);
                planet.Select();
            }
        }
    }
}