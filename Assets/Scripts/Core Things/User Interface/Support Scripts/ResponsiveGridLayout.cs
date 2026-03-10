using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Автоматически адаптирует Cell Size у GridLayoutGroup под разрешение экрана.
/// Прикрепи этот скрипт к тому же GameObject, что и GridLayoutGroup.
/// </summary>
[RequireComponent(typeof(GridLayoutGroup))]
public class ResponsiveGridLayout : MonoBehaviour
{
    [Header("Колонки и строки")]
    [Tooltip("Количество колонок. 0 = авто по ширине")]
    [SerializeField] private int columns = 3;

    [Tooltip("Количество строк. 0 = авто по высоте (используется только если columns = 0)")]
    [SerializeField] private int rows = 0;

    [Header("Соотношение сторон ячейки")]
    [Tooltip("Соотношение ширины к высоте ячейки (например 1 = квадрат, 0.5 = вдвое выше)")]
    [SerializeField] private float cellAspectRatio = 1f;

    [Header("Отступы (в % от ширины контейнера)")]
    [Tooltip("Учитывать padding GridLayoutGroup при расчёте")]
    [SerializeField] private bool respectPadding = true;

    [Tooltip("Обновлять размер при каждом изменении размера экрана (через RectTransform)")]
    [SerializeField] private bool updateOnResize = true;

    private GridLayoutGroup _grid;
    private RectTransform _rect;
    private Vector2 _lastSize;

    private void Awake()
    {
        _grid = GetComponent<GridLayoutGroup>();
        _rect = GetComponent<RectTransform>();
    }

    private void Start()
    {
        UpdateCellSize();
        _lastSize = _rect.rect.size;
    }

    private void Update()
    {
        if (!updateOnResize) return;

        Vector2 currentSize = _rect.rect.size;
        if (currentSize != _lastSize)
        {
            _lastSize = currentSize;
            UpdateCellSize();
        }
    }

    [ContextMenu("Update Cell Size Now")]
    public void UpdateCellSize()
    {
        if (_grid == null) _grid = GetComponent<GridLayoutGroup>();
        if (_rect == null) _rect = GetComponent<RectTransform>();

        float containerWidth = _rect.rect.width;
        float containerHeight = _rect.rect.height;

        if (containerWidth <= 0 || containerHeight <= 0) return;

        float paddingH = respectPadding ? _grid.padding.left + _grid.padding.right : 0;
        float paddingV = respectPadding ? _grid.padding.top + _grid.padding.bottom : 0;

        float usableWidth = containerWidth - paddingH;
        float usableHeight = containerHeight - paddingV;

        float cellWidth, cellHeight;

        if (columns > 0)
        {
            // Считаем по количеству колонок
            float spacingX = _grid.spacing.x * (columns - 1);
            cellWidth = (usableWidth - spacingX) / columns;
            cellHeight = cellWidth / cellAspectRatio;
        }
        else if (rows > 0)
        {
            // Считаем по количеству строк
            float spacingY = _grid.spacing.y * (rows - 1);
            cellHeight = (usableHeight - spacingY) / rows;
            cellWidth = cellHeight * cellAspectRatio;
        }
        else
        {
            Debug.LogWarning("[ResponsiveGridLayout] Укажи columns или rows больше 0.");
            return;
        }

        cellWidth = Mathf.Max(cellWidth, 1f);
        cellHeight = Mathf.Max(cellHeight, 1f);

        _grid.cellSize = new Vector2(cellWidth, cellHeight);
    }

    private void OnValidate()
    {
        // Обновление прямо в редакторе при изменении параметров
        if (_grid == null) _grid = GetComponent<GridLayoutGroup>();
        if (_rect == null) _rect = GetComponent<RectTransform>();
        if (_grid != null && _rect != null && _rect.rect.width > 0)
            UpdateCellSize();
    }
}
