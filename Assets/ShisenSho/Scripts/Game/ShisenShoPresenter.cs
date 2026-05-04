using Hai;
using ShisenSho.InformationUI;
using ShisenSho.Result;
using UnityEngine;

namespace ShisenSho.Game
{
    public class ShisenShoPresenter : MonoBehaviour
    {
        [SerializeField] private ShisenShoUseCase _useCase = null;
        [SerializeField] private ShisenShoView _view = null;
        [SerializeField] private GameObject _haiPrefab = null;
        [SerializeField] private ShisenShoHaiSelector _selector = null;
        [SerializeField] private ResultPresenter _resultPresenter = null;

        [SerializeField]
        private InformationPresenter _informationPresenter;
    
    
        private int selectIndex1 = -1;
        private int selectIndex2 = -1;
    
        private void Awake()
        {
        }

        // Start is called before the first frame update
        private void Start()
        {
            _useCase.Create();
            CreateBoard();
        
            _resultPresenter.Hide();
        }


        private void CreateBoard()
        {
            int index = 0;
            int boardIndex = 0;
        
            for(int i=0; i< _useCase.W * _useCase.H; i++)
            {
                var hai = _useCase.State.board[i];
                int W = 17;
                int H = 8;
                int xi = i % _useCase.W;
                int yi = i / _useCase.W;
                boardIndex++;
            
//            Debug.Log($"i={index.ToString()} v={hai.ToString()},");
//            if (DrawHaiSprite(hai, xi, W, yi, H, index, out var haiObj)) continue;
                if (DrawHai(hai, xi, W, yi, H, index, out var haiObj,boardIndex))
                {
                    continue;
                }

                index++;
            }

 
        }

        private bool DrawHai(int hai, int xi, int W, int yi, int H, int index, out GameObject haiObj, int boardIndex)
        {
            string prefabName = ShisenShoHelper.GetPrefabName(hai);
        
            var haiPrefab = Resources.Load(prefabName) as GameObject;
            if (haiPrefab == null)
            {
                Debug.Log( "DrawHai Resources.Load: " + prefabName );
                haiObj = null;
                return false; 
            }
            haiObj = Instantiate(haiPrefab, _view.HaiParent.transform);
            if (haiObj == null)
            {
                Debug.Log( "DrawHai Instantiate: " + prefabName );
                haiObj = null;
                return false;
            }
        
            Vector3 rotation = new Vector3(0.0f, 180.0f, 0.0f);
        
            var position = GetHaiPosition(boardIndex-1 );
            haiObj.transform.localRotation = Quaternion.Euler( rotation);
            haiObj.transform.position = position;
            var haiData = haiObj.GetComponent<HaiData>();
            if (haiData != null)
            {
                haiData.HaiValue = hai;
                haiData.IndexX = xi;
                haiData.IndexY = yi;
                haiData.Index = index;
                haiData.BoardIndex = boardIndex - 1;

            }
   
            return true;
        }

        public Vector3 GetHaiPosition(int boardIndex)
        {
            float spriteW = 0.308f;
            float spriteH = 0.405f;
            int W = 17;
            int H = 8;
            int xi = boardIndex % _useCase.W;
            int yi = boardIndex / _useCase.W;

            float offsetX = -0.61f;
            float offsetZ = -0.55f;
        
            float x = xi * spriteW - W / 2 * spriteW + offsetX;
            float z = yi * spriteH - H / 2 * spriteH + offsetZ;
            float y = 0.0f;
            Vector3 position = new Vector3(x, y, z);
            return position;
        }

        void DestroyBoard()
        {
            foreach ( Transform n in _view.HaiParent.transform )
            {
                GameObject.Destroy(n.gameObject);
            }
        }

        void UpdateBoard()
        {
            if (_useCase.State.target >= 0)
            {
                _selector.UpdateCursor(_useCase.State.target);
            }
            else
            {
                _selector.UpdateCursor(_useCase.State.target);
            }
        
            DestroyBoard();
            CreateBoard();

            if (_useCase.State.rest <= 0)
            {
                _resultPresenter.Show();
            }
        }

   
        private void Update()
        {
            // Unity上での操作取得
            if (Application.isEditor)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    var _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(_ray, out hit))
                    {
                        OnTouchHai(hit);
                    }
                }
            }
            // 端末上での操作取得
            else
            {
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.touches[0];
                    if (touch.phase == TouchPhase.Began)
                    {
                        var _ray = Camera.main.ScreenPointToRay(touch.position);
                        RaycastHit hit;
                        if (Physics.Raycast(_ray, out hit))
                        {
                            OnTouchHai(hit);
                        }
                    }
                }
            }
        }

        private void OnTouchHai(RaycastHit hit)
        {
            var haiData = hit.collider.gameObject.GetComponent<HaiData>();
            if (!haiData)
            {
                return;
            }
            // Debug.Log($"{hit.collider.gameObject.name}をクリック中 v={haiData.HaiValue.ToString()}");
            if (selectIndex2 != -1)
            {
            }

            var state = _useCase.State;
            if (selectIndex1 == -1)
            {
                selectIndex2 = haiData.Index;
                // TODO ジャッジ
                state = _useCase.UpdateState(haiData.BoardIndex);

            
                selectIndex1 = -1;
                selectIndex2 = -1;
            }
            else
            {
                selectIndex1 = haiData.Index;
                state = _useCase.UpdateState(haiData.BoardIndex);
            }

            if (!state.solved && state.tested)
            {
                _informationPresenter.Show();
            }
        
            UpdateBoard();
        }
    }
}
