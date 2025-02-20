using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Difficulty
{
    easy,
    medium,
    hard,
}
public class Room : MonoBehaviour
{
    public RoomController[] topRooms;
    public RoomController[] rightRooms;
    public RoomController[] bottomRooms;
    public RoomController[] leftRooms;

    public RoomController centreRoom;
    public RoomController topRoom;
    public RoomController rightRoom;
    public RoomController bottomRoom;
    public RoomController leftRoom;
    public RoomController leftRightRoom;
    public RoomController leftBottomRoom;
    public RoomController rightBottomRoom;
    public RoomController topBottomRoom;
    public RoomController topLeftRoom;
    public RoomController topRightRoom;
    public RoomController topRightLeftRoom;
    public RoomController topRightBottomRoom;
    public RoomController topBottomLeftRoom;
    public RoomController rightBottomLeftRoom;

    public Direction spawnDirection;
    public RoomController activeRoom;
    public Difficulty difficulty = Difficulty.easy;
    private RoomManager roomManager;

    private void Awake()
    {
        roomManager = FindObjectOfType<RoomManager>();
        roomManager.currentRoomCount.Add(this);
    }
    private void OnEnable()
    {
    }

    public void SetActiveRoom(RoomShape roomShape)
    {
        DisableAllRooms();
        switch (roomShape)
        {
            case RoomShape.Centre:
                activeRoom = centreRoom;
                spawnDirection = Direction.Centre;
                break;
            case RoomShape.T:
                activeRoom = topRoom;
                break;
            case RoomShape.R:
                activeRoom = rightRoom;
                break;
            case RoomShape.B:
                activeRoom = bottomRoom;
                break;
            case RoomShape.L:
                activeRoom = leftRoom;
                break;
            case RoomShape.TR:
                activeRoom = topRightRoom;
                break;
            case RoomShape.TB:
                activeRoom = topBottomRoom;
                break;
            case RoomShape.TL:
                activeRoom = topLeftRoom;
                break;
            case RoomShape.RB:
                activeRoom = rightBottomRoom;
                break;
            case RoomShape.LR:
                activeRoom = leftRightRoom;
                break;
            case RoomShape.LB:
                activeRoom = leftBottomRoom;
                break;
            case RoomShape.TRB:
                activeRoom = topRightBottomRoom;
                break;
            case RoomShape.TRL:
                activeRoom = topRightLeftRoom;
                break;
            case RoomShape.TBL:
                activeRoom = topBottomLeftRoom;
                break;
            case RoomShape.RBL:
                activeRoom = rightBottomLeftRoom;
                break;
        }
        activeRoom.gameObject.SetActive(true);
    }
    public void SetCentreRoom()
    {
        DisableAllRooms();
        activeRoom = centreRoom;
        activeRoom.gameObject.SetActive(true);
    }
    public void SetActiveRoomRandom(Direction direction)
    {
        int random;
        DisableAllRooms();

        switch (direction)
        {
            case Direction.Centre:
                activeRoom = centreRoom;
                break;
            case Direction.Top:
                random = Random.Range(0, bottomRooms.Length);
                activeRoom = bottomRooms[random];
                break;
            case Direction.Right:
                random = Random.Range(0, leftRooms.Length);
                activeRoom = leftRooms[random];
                break;
            case Direction.Bottom:
                random = Random.Range(0, topRooms.Length);
                activeRoom = topRooms[random];
                break;
            case Direction.Left:
                random = Random.Range(0, rightRooms.Length);
                activeRoom = rightRooms[random];
                break;
        }
        spawnDirection = direction;
        activeRoom.gameObject.SetActive(true);
    }
    void DisableAllRooms()
    {
        centreRoom.gameObject.SetActive(false);
        topRoom.gameObject.SetActive(false);
        rightRoom.gameObject.SetActive(false);
        bottomRoom.gameObject.SetActive(false);
        leftRoom.gameObject.SetActive(false);
        leftRightRoom.gameObject.SetActive(false);
        rightBottomRoom.gameObject.SetActive(false);
        topBottomRoom.gameObject.SetActive(false);
        topLeftRoom.gameObject.SetActive(false);
        topRightRoom.gameObject.SetActive(false);
        topRightLeftRoom.gameObject.SetActive(false);
        topRightBottomRoom.gameObject.SetActive(false);
        topBottomLeftRoom.gameObject.SetActive(false);
        rightBottomLeftRoom.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
