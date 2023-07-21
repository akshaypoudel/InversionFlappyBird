using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {

    private const float CAMERA_ORTHO_SIZE = 50f;
    private const float PIPE_WIDTH = 7.8f;
    private const float PIPE_HEAD_HEIGHT = 3.75f;
    private const float PIPE_MOVE_SPEED = 30f;
    private const float PIPE_DESTROY_X_POSITION = -300f;
    private const float PIPE_SPAWN_X_POSITION = +120f;
    private const float BIRD_X_POSITION = 0f;
    private const float PIPE_BODY_HEIGHT_IF_NOT_BOTTOM=100f;

    private static Level instance;

    public static Level GetInstance()=>instance;
    private List<Pipe> pipeList;
    private int pipesPassedCount;
    private int pipesSpawned;
    private float pipeSpawnTimer;
    private float pipeSpawnTimerMax;
    private float gapSize;

    public enum Difficulty 
    {
        Easy,
        Medium,
        Hard,
        Impossible,
    }

    private void Awake() 
    {
        instance = this;
        pipeList = new List<Pipe>();
        pipeSpawnTimerMax = 1f;
        SetDifficulty(Difficulty.Easy);
    }


    private void Update() 
    {
        if(Bird.GetInstance().state==Bird.State.Playing)
        {
            HandlePipeMovement();
            HandlePipeSpawning();
        }
    }
    private void HandlePipeSpawning() 
    {
        pipeSpawnTimer -= Time.deltaTime;
        if (pipeSpawnTimer < 0) {
            // Time to spawn another Pipe
            pipeSpawnTimer += pipeSpawnTimerMax;
            
            float heightEdgeLimit = 20f;
            float minHeight = gapSize * .5f + heightEdgeLimit;
            float totalHeight = CAMERA_ORTHO_SIZE * 2f;
            float maxHeight = totalHeight - gapSize * .5f - heightEdgeLimit;

            float height = Random.Range(minHeight, maxHeight);
            CreateGapPipes(height, gapSize, PIPE_SPAWN_X_POSITION);
        }
    }

    private void HandlePipeMovement() 
    {
        for (int i=0; i<pipeList.Count; i++) {
            Pipe pipe = pipeList[i];

            bool isToTheRightOfBird = pipe.GetXPosition() > BIRD_X_POSITION;
            pipe.Move();
            if (isToTheRightOfBird && pipe.GetXPosition() <= BIRD_X_POSITION && pipe.IsBottom()) 
            {
                pipesPassedCount++;
                SoundManager.PlaySound(SoundManager.Sound.Score,0.3f);
            }

            if (pipe.GetXPosition() < PIPE_DESTROY_X_POSITION) 
            {
                pipe.DestroySelf();
                pipeList.Remove(pipe);
                i--;
            }
        }
    }

    private void CreateGapPipes(float gapY, float gapSize, float xPosition) 
    {
        CreatePipe(gapY - gapSize * .5f, xPosition, true);
        CreatePipe(CAMERA_ORTHO_SIZE * 2f - gapY - gapSize * .5f, xPosition, false);
        pipesSpawned++;
        SetDifficulty(GetDifficulty());
    }

    private void CreatePipe(float height, float xPosition, bool createBottom)
    {
        // Set up Pipe Head
        Transform pipeHead;
        float pipeHeadYPosition;
        SetUpPipeHead(height, xPosition, createBottom, out pipeHead, out pipeHeadYPosition);
        // Set up Pipe Body

        Transform pipeBody = SetUpPipeBody(xPosition, createBottom, pipeHeadYPosition);


        SetSpriteSize(height, createBottom, pipeBody);

        SetColliderHeight(height, createBottom, pipeBody);

        Pipe pipe = new Pipe(pipeHead, pipeBody, createBottom);
        pipeList.Add(pipe);
    }

    private void SetUpPipeHead(float height, float xPosition, bool createBottom, out Transform pipeHead, out float pipeHeadYPosition)
    {
        pipeHead = Instantiate(GameAssets.GetInstance().pfPipeHead);
        if (createBottom)
        {
            pipeHeadYPosition = -CAMERA_ORTHO_SIZE + height - PIPE_HEAD_HEIGHT * .5f;
        }
        else
        {
            pipeHeadYPosition = +CAMERA_ORTHO_SIZE - height + PIPE_HEAD_HEIGHT * .5f;
        }
        pipeHead.position = new Vector3(xPosition, pipeHeadYPosition);
    }
    private Transform SetUpPipeBody(float xPosition, bool createBottom, float pipeHeadYPosition)
    {
        Transform pipeBody = Instantiate(GameAssets.GetInstance().pfPipeBody);
        float pipeBodyYPosition;
        if (createBottom)
        {
            pipeBodyYPosition = -CAMERA_ORTHO_SIZE;
        }
        else
        {
            pipeBodyYPosition = +CAMERA_ORTHO_SIZE * 2 + pipeHeadYPosition;
            pipeBody.localScale = new Vector3(1, -1, 1);
        }
        pipeBody.position = new Vector3(xPosition, pipeBodyYPosition);
        return pipeBody;
    }

    private void SetSpriteSize(float height, bool createBottom, Transform pipeBody)
    {
        SpriteRenderer pipeBodySpriteRenderer = pipeBody.GetComponent<SpriteRenderer>();

        if (createBottom)
            pipeBodySpriteRenderer.size = new Vector2(PIPE_WIDTH, height);
        else
            pipeBodySpriteRenderer.size = new Vector2(PIPE_WIDTH, PIPE_BODY_HEIGHT_IF_NOT_BOTTOM);
    }

    private void SetColliderHeight(float height, bool createBottom, Transform pipeBody)
    {
        BoxCollider2D pipeBodyBoxCollider = pipeBody.GetComponent<BoxCollider2D>();
        pipeBodyBoxCollider.size = new Vector2(PIPE_WIDTH, height);
        pipeBodyBoxCollider.offset = new Vector2(0f, height * .5f);
        if (!createBottom)
        {
            pipeBodyBoxCollider.size = new Vector2(PIPE_WIDTH, PIPE_BODY_HEIGHT_IF_NOT_BOTTOM);
            pipeBodyBoxCollider.offset = new Vector2(0f, PIPE_BODY_HEIGHT_IF_NOT_BOTTOM * .5f);
        }
    }


    private void SetDifficulty(Difficulty difficulty) 
    {
        switch (difficulty) {
        case Difficulty.Easy:
            gapSize = 40f;
            pipeSpawnTimerMax = 1.3f;
            break;
        case Difficulty.Medium:
            gapSize = 30f;
            pipeSpawnTimerMax = 1.2f;
            break;
        case Difficulty.Hard:
            gapSize = 25f;
            pipeSpawnTimerMax = 1.1f;
            break;
        case Difficulty.Impossible:
            gapSize = 20f;
            pipeSpawnTimerMax = 1.0f;
            break;
        }
    }

    private Difficulty GetDifficulty() 
    {
        if (pipesSpawned >= 18) return Difficulty.Impossible;
        if (pipesSpawned >= 10) return Difficulty.Hard;
        if (pipesSpawned >= 5) return Difficulty.Medium;
        return Difficulty.Easy;
    }

    public int GetPipesSpawned()=>pipesSpawned;

    public int GetPipesPassedCount()=>pipesPassedCount;


    private class Pipe 
    {
        private Transform pipeHeadTransform;
        private Transform pipeBodyTransform;
        private bool isBottom;

        public Pipe(Transform pipeHeadTransform, Transform pipeBodyTransform, bool isBottom) 
        {
            this.pipeHeadTransform = pipeHeadTransform;
            this.pipeBodyTransform = pipeBodyTransform;
            this.isBottom = isBottom;
        }

        public void Move() 
        {
            pipeHeadTransform.position += new Vector3(-1, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime;
            pipeBodyTransform.position += new Vector3(-1, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime;
        }

        public float GetXPosition()=>pipeHeadTransform.position.x;
        public bool IsBottom()=>isBottom;
        public void DestroySelf() 
        {
            Destroy(pipeHeadTransform.gameObject);
            Destroy(pipeBodyTransform.gameObject);
        }

    }

}

