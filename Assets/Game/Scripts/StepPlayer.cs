using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepPlayer : MonoBehaviour
{

    private struct RunningStep
    {
        public GameObject parent;
        public float startTimestamp;
        public Step step;
        public GameObject markInstance;
    }

    public GameObject[] componentList;

    public StepList playingList;
    public int stepIndex;
    public float playTime;
    public float nextStepTime;

    private List<RunningStep> currentSteps;

    public GameObject stepmarkPrefab;
    public GameObject stepmarkSliderPrefab;
    public GameObject stepmarTurntablePrefab;
    public GameObject stepmarkButtonPrefab;

    public bool songFinished { get; private set; } = true;
    public bool loopSteps;

    public static float HEALTH_MISS = -0.1f;
    public static float HEALTH_PERFECT = 0.02f;
    public static float PERFECT_TIME_DELTA = 0.3f;
    public static float PERFECT_VALUE_DELTA = 0.2f;
    public static float MISS_TURNTABLE_DELTA = 0.3f;

    public void PlaySteps(StepList stepList)
    {
        currentSteps = new List<RunningStep>();

        playingList = stepList;
        songFinished = false;
        stepIndex = 0;
        playTime = 0;
        nextStepTime = 0;
    }

    private void Update()
    {
        playTime += Time.deltaTime;

        if (!songFinished)
        {
            nextStepTime += Time.deltaTime;
            Step step = playingList.steps[stepIndex];
            float startTime = step.startTime;
            if (nextStepTime > startTime)
            {
                nextStepTime -= startTime;
                Transform componentTransform = componentList[(int)step.component].transform;
                GameObject markInstance = null;
                Vector3 startPosition = new Vector3(0, 4, 0);
                Vector3 endPosition = Vector3.zero;

                InteractButton interactButton = componentTransform.GetComponent<InteractButton>();
                InteractSlider interactSlider = componentTransform.GetComponent<InteractSlider>();
                InteractTurntable interactTurntable = componentTransform.GetComponent<InteractTurntable>();
                if (interactButton != null) {
                    markInstance = Instantiate(stepmarkButtonPrefab, componentTransform);
                }
                else if (interactSlider != null)
                {
                    markInstance = Instantiate(stepmarkSliderPrefab, componentTransform);
                    startPosition += interactSlider.GetValueOffset(step.value);
                    endPosition += interactSlider.GetValueOffset(step.value);
                }
                else if(interactTurntable != null)
                {
                    markInstance = Instantiate(stepmarTurntablePrefab, componentTransform);
                    markInstance.transform.localScale = new Vector3(1, step.duration, 1);
                    startPosition += new Vector3(0, step.duration, 0);
                    endPosition += new Vector3(0, step.duration, 0);
                }
                else
                {
                    markInstance = Instantiate(stepmarkPrefab, componentTransform);
                }

                markInstance.transform.localPosition = startPosition;

                RunningStep runningStep = new RunningStep();
                runningStep.parent = componentTransform.gameObject;
                runningStep.startTimestamp = playTime;
                runningStep.step = step;
                runningStep.markInstance = markInstance;
                currentSteps.Add(runningStep);

                LeanTween.moveLocal(markInstance, endPosition, step.perfectTime).setOnComplete(
                    () => {
                        if (interactTurntable)
                        {
                            LeanTween.moveLocal(runningStep.markInstance, new Vector3(0, -step.duration, 0), step.duration).setOnComplete(
                                () =>
                                {
                                    StepPerfect();
                                    currentSteps.Remove(runningStep);
                                    LeanTween.scale(runningStep.markInstance, Vector3.one * 2, PERFECT_TIME_DELTA).setOnComplete(
                                        () =>
                                        {
                                            Destroy(runningStep.markInstance);
                                        }
                                    );
                                }
                            );
                        }
                        else
                        {
                            LeanTween.scale(runningStep.markInstance, Vector3.zero, PERFECT_TIME_DELTA).setOnComplete(
                                () =>
                                {
                                    if (interactButton != null)
                                    {
                                        StepMiss();
                                    }
                                    else if (interactSlider != null)
                                    {
                                        float deltaValue = step.value - interactSlider.value;
                                        if (Mathf.Abs(deltaValue) < PERFECT_VALUE_DELTA)
                                        {
                                            StepPerfect();
                                        }
                                        else
                                        {
                                            StepMiss();
                                        }
                                    }

                                    Destroy(runningStep.markInstance);
                                    currentSteps.Remove(runningStep);
                                }
                            );
                        }
                    }
                );

                ++stepIndex;
                if (stepIndex == playingList.steps.Length)
                {
                    if(loopSteps)
                    {
                        stepIndex = 0;
                        nextStepTime = 0;
                    }
                    else
                    {
                        songFinished = true;
                    }
                }
            }
        }
    }

    private RunningStep GetFirstRunningStep(GameObject parent)
    {
        for(int stepIndex = 0; stepIndex < currentSteps.Count; ++stepIndex)
        {
            RunningStep runningStep = currentSteps[stepIndex];
            if(runningStep.parent == parent)
            {
                return runningStep;
            }
        }
        return new RunningStep();
    }

    private void StepPerfect()
    {
        Game.data.health += HEALTH_PERFECT;
        Game.data.perfects++;

        for(int characterIndex = 0; characterIndex < Game.data.characterCount; ++characterIndex)
        {
            Character character = Game.data.characters[characterIndex];
            character.StartDance();
        }
    }

    private void StepMiss()
    {
        Game.data.health += HEALTH_MISS;
        Game.data.misses++;

        for (int characterIndex = 0; characterIndex < Game.data.characterCount; ++characterIndex)
        {
            Character character = Game.data.characters[characterIndex];
            character.StopDance();
        }
    }

    public void ButtonPressed(InteractButton interact)
    {
        RunningStep runningStep = GetFirstRunningStep(interact.gameObject);
        if (runningStep.parent != null)
        {
            LeanTween.cancel(runningStep.markInstance);

            Step step = runningStep.step;
            float deltaTime = playTime - (runningStep.startTimestamp + step.perfectTime);
            if (Mathf.Abs(deltaTime) < PERFECT_TIME_DELTA)
            {
                StepPerfect();
                LeanTween.scale(runningStep.markInstance, Vector3.one * 2, PERFECT_TIME_DELTA).setOnComplete(
                    () =>
                    {
                        Destroy(runningStep.markInstance);
                        currentSteps.Remove(runningStep);
                    }
                );
            }
            else
            {
                StepMiss();
                LeanTween.scale(runningStep.markInstance, Vector3.zero, PERFECT_TIME_DELTA).setOnComplete(
                    () =>
                    {
                        Destroy(runningStep.markInstance);
                        currentSteps.Remove(runningStep);
                    }
                );
            }
        }
    }

    public void TurntableMiss(InteractTurntable interact)
    {
        RunningStep runningStep = GetFirstRunningStep(interact.gameObject);
        if (runningStep.parent != null)
        {
            if(playTime > runningStep.startTimestamp + runningStep.step.perfectTime * 1.2f) {
                LeanTween.cancel(runningStep.markInstance);

                Step step = runningStep.step;
                StepMiss();
                LeanTween.scale(runningStep.markInstance, Vector3.zero, PERFECT_TIME_DELTA).setOnComplete(
                    () =>
                    {
                        Destroy(runningStep.markInstance);
                        currentSteps.Remove(runningStep);
                    }
                );
            }
        }
    }

}
