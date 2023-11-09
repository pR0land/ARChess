using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ChessPieceInteractor : MonoBehaviour
{
    private Camera _camera; 
    private GameObject _selectedPiece; //Used to store the current selected chess piece
    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        //Select chesspiece logic
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            //Detect touch position (In screen pixel coordinates) and create a ray from it 
            Ray ray = _camera.ScreenPointToRay(Input.touches[0].position);
            RaycastHit hit;
            //Debug.Log($"Touch detected at {Input.touches[0].position}");    
            if (Physics.Raycast(ray, out hit)) //returns true if the ray hits something and stores the collision position in hit
            {
                //Debug.Log($"Log: Object hit at {hit.point}");
                Debug.Log($"Log: Object hit name: {hit.collider.gameObject.name}");
                if (hit.collider.gameObject != null && hit.collider.gameObject.CompareTag("ChessPiece"))
                {
                    Debug.Log("Log: Tag comparison successful");
                    
                    if (_selectedPiece != null && hit.collider.gameObject != _selectedPiece)
                    {
                        DeselectPiece();
                        SelectPiece(hit.collider.gameObject);
                    }
                    else if (_selectedPiece == null)
                    {   
                        
                        SelectPiece(hit.collider.gameObject);
                    }
                }
                
            } 
        }
        
        //Move chesspiece logic
        if (_selectedPiece != null && Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Moved)
        {
            
            Ray ray = _camera.ScreenPointToRay(Input.touches[0].position);
            RaycastHit hit;
            if (Physics.Raycast(ray,out hit,math.INFINITY,LayerMask.GetMask("ChessboardSurface")))
            {
                if (hit.collider.gameObject.CompareTag("ChessPiece")){}
                {   
                    MoveChessPiece(hit);
                }
            }
        }
        
        //Stop moving chesspiece logic
        if (_selectedPiece != null && Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended)
        {
            DeselectPiece();
        }
    }
    
    private void SelectPiece(GameObject piece)
    {
        //Debug.Log("Log: Piece selected");
        _selectedPiece = piece;
        //_selectedPiece.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.red;
        //_selectedPiece.GetComponent<Material>().color = Color.red;
    }

    private void DeselectPiece()
    {
        //Debug.Log("Log: Piece deselected");
        //Get child gameobject and set its color to white
        //_selectedPiece.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.white;
        
        //_selectedPiece.GetComponent<Material>().color = Color.white;
        _selectedPiece = null;
    }

    private void MoveChessPiece(RaycastHit hit)
    {
        _selectedPiece.transform.position = hit.point;
        //Vector3 curPos = _camera.ScreenToViewportPoint(Input.touches[0].position);
        //Vector3 lastPos = _camera.ScreenToViewportPoint(Input.touches[0].position - Input.touches[0].deltaPosition);
        //Vector3 touchDir = curPos - lastPos;
        Debug.Log($"Log: Moving Chesspiece to {hit.point}");
        //touchDir.y = 0;

        //_selectedPiece.transform.position = touchDir;
        


    }
}