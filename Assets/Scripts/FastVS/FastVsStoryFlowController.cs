using Anemora.TimeManagement;
using UnityEngine;

namespace Anemora.FastVS
{
    public sealed class FastVsStoryFlowController : MonoBehaviour
    {
        private const float QuestionHeadWorldOffset = 1.46f;

        private enum StoryMode
        {
            None,
            OpeningWake,
            DoorBrushBeat,
            RetoDialogue,
            AriaDialogue
        }

        private enum RetoSequence
        {
            None,
            OpeningAndActivation,
            PastObservation,
            ReturnAndHint
        }

        private readonly struct StoryStep
        {
            public StoryStep(string beatId, string speaker, string text, FastVsRetoWritingState poseState = FastVsRetoWritingState.DialogueIdle)
            {
                BeatId = beatId;
                Speaker = speaker;
                Text = text;
                PoseState = poseState;
                IsPause = false;
                PauseSeconds = 0f;
            }

            public StoryStep(string beatId, float pauseSeconds, FastVsRetoWritingState poseState = FastVsRetoWritingState.DialogueIdle)
            {
                BeatId = beatId;
                Speaker = string.Empty;
                Text = string.Empty;
                PoseState = poseState;
                IsPause = true;
                PauseSeconds = pauseSeconds > 0.01f ? pauseSeconds : 0.01f;
            }

            public string BeatId { get; }
            public string Speaker { get; }
            public string Text { get; }
            public FastVsRetoWritingState PoseState { get; }
            public bool IsPause { get; }
            public float PauseSeconds { get; }
        }

        [SerializeField] private TimeWindowPairedSpacePortalController portalController;
        [SerializeField] private FastVsHouseAreaVisibility areaVisibility;
        [SerializeField] private FastVsVisualDirectionGuide movementGuide;
        [SerializeField] private CharacterController playerController;
        [SerializeField] private Transform player;
        [SerializeField] private Camera storyCamera;
        [SerializeField] private FastVsRetoWritingAnimator retoAnimator;
        [SerializeField] private FastVsStoryDialoguePresenter dialoguePresenter;
        [SerializeField] private FastVsStoryRuntimeHud runtimeHud;
        [SerializeField] private GameObject currentDeskBookObject;
        [SerializeField] private GameObject currentTimeWindowBookCueObject;
        [SerializeField] private GameObject currentTimeWindowAriaCueObject;
        [SerializeField] private GameObject timewriterPocketGlowObject;
        [SerializeField] private GameObject pastTargetBookObject;
        [SerializeField] private GameObject pastTargetBookMarkerObject;
        [SerializeField] private GameObject pastAriaMarkerObject;
        [SerializeField] private Vector3 retoLocalPosition;
        [SerializeField] private float retoInteractionRadius = 1.65f;
        [SerializeField] private Vector3 pastLibraryBookLocalPosition = new Vector3(31.94f, 0.02f, 20.12f);
        [SerializeField] private float pastBookInteractionRadius = 1.20f;
        [SerializeField] private Vector3 pastLibraryAriaLocalPosition = new Vector3(28.02f, 0.02f, 21.42f);
        [SerializeField] private float pastAriaInteractionRadius = 1.20f;
        [SerializeField] private Vector3 doorBrushBeatTriggerLocalCenter = new Vector3(-7.53f, 0.70f, -10.16f);
        [SerializeField] private Vector3 doorBrushBeatTriggerLocalSize = new Vector3(0.86f, 1.72f, 0.34f);
        [SerializeField] private bool showOpeningHint = true;

        private static readonly StoryStep[] RetoOpeningSteps =
        {
            new StoryStep("scene1.reto.1b.initial", "レト", "...見ない顔ですね。"),
            new StoryStep("scene1.reto.1b.initial", "レト", "私はレト。元、教師でした。"),
            new StoryStep("scene1.reto.1b.initial", "レト", "今は、ここで街の記録を残しています。"),
            new StoryStep("scene1.reto.1c.library_history", "レト", "ここは昔、街の記録がすべて集まる場所だったそうです。"),
            new StoryStep("scene1.reto.1c.library_history", "レト", "人々の名簿、家系の記録、街の決まり事..."),
            new StoryStep("scene1.reto.1c.library_history", "レト", "今では、誰も覚えていません。"),
            new StoryStep("scene1.reto.1c.library_history", "ニロ", "(...誰も)"),
            new StoryStep("scene1.reto.1c.library_history", "レト", "私が書き写しているのも、聞いた話の継ぎはぎです。"),
            new StoryStep("scene1.reto.1c.library_history", "レト", "それでも、書いておかないと、いずれ何もかもが...", FastVsRetoWritingState.LookingUp),
            new StoryStep("scene1.reto.1c.library_history.pause_before_resolve_to_record", 1.85f, FastVsRetoWritingState.LookingUp),
            new StoryStep("scene1.reto.1c.library_history.resolve_to_record", "レト", "いえ。今のは、ただの独り言です。", FastVsRetoWritingState.LookingUp),
            new StoryStep("scene1.reto.1d.timewriter_activation.pocket_glow_pause", 3.65f, FastVsRetoWritingState.LookingUp),
            new StoryStep("scene1.reto.1d.timewriter_activation", "ニロ", "(筆が...!)"),
            new StoryStep("scene1.reto.1d.timewriter_activation.pause_after_pocket", 1.90f),
            new StoryStep("scene1.reto.1d.timewriter_activation", "レト", "...?"),
            new StoryStep("scene1.reto.1d.timewriter_activation.pause_before_past_control", 2.10f, FastVsRetoWritingState.LookingUp)
        };

        private static readonly StoryStep[] RetoPastObservationSteps =
        {
            new StoryStep("scene1.reto.1e.past_library_observation.book_location", "ニロ", "(...ここに、本が)"),
            new StoryStep("scene1.reto.1e.past_library_observation.pause_before_take_book", 1.60f),
            new StoryStep("scene1.reto.1e.past_library_observation.take_book", "ニロ", "(...本を、見つけた)")
        };

        private static readonly StoryStep[] AriaPastSteps =
        {
            new StoryStep("scene1.reto.1e.past_library_observation.aria.notice_person", "ニロ", "(...人)"),
            new StoryStep("scene1.reto.1e.past_library_observation.aria.pause_before_monologue", 1.35f),
            new StoryStep("scene1.reto.1e.past_library_observation.aria.niro_observe_1", "ニロ", "(...本を読んでいる)"),
            new StoryStep("scene1.reto.1e.past_library_observation.aria.pause_2", 1.05f),
            new StoryStep("scene1.reto.1e.past_library_observation.aria.niro_observe_2", "ニロ", "(...こちらには気づいていない)")
        };

        private static readonly StoryStep[] RetoReturnAndHintSteps =
        {
            new StoryStep("scene1.reto.1f.return_present.show_book", "ニロ", "(...本を、レトに見せる)"),
            new StoryStep("scene1.reto.1f.return_present.pause_before_question", 1.25f),
            new StoryStep("scene1.reto.1f.return_present.question", "レト", "...?"),
            new StoryStep("scene1.reto.1f.return_present.pause_after_question", 2.05f),
            new StoryStep("scene1.reto.1f.return_present.reaction", "レト", "...本物だ", FastVsRetoWritingState.LookingUp),
            new StoryStep("scene1.reto.1f.return_present.face_drop_motion", 1.05f, FastVsRetoWritingState.Lowering),
            new StoryStep("scene1.reto.1f.return_present.face_lift_motion", 1.05f, FastVsRetoWritingState.Raising),
            new StoryStep("scene1.reto.1f.return_present.pause_before_acceptance", 1.45f, FastVsRetoWritingState.LookingUp),
            new StoryStep("scene1.reto.1f.return_present.acceptance", "レト", "...そうですか。", FastVsRetoWritingState.LookingUp),
            new StoryStep("scene1.reto.1f.return_present.pause_after_acceptance", 1.35f, FastVsRetoWritingState.LookingUp),
            new StoryStep("scene1.reto.1f.return_present.appreciation", "レト", "...あなたのような方が、来てくれるとは。"),
            new StoryStep("scene1.reto.1g.mia_hint.setup", "レト", "...そういえば。"),
            new StoryStep("scene1.reto.1g.mia_hint.notice", "レト", "中央集落のミアさんが、今朝、困っていました。"),
            new StoryStep("scene1.reto.1g.mia_hint.help", "レト", "あなたなら、力になれるかもしれません。")
        };

        private StoryMode mode;
        private RetoSequence activeRetoSequence;
        private FastVsHouseArea lastArea;
        private string currentBeatId = "opening.house_interior";
        private int ignoreInputFrame = -1;
        private int doorBeatPage;
        private int retoStepIndex = -1;
        private int ariaStepIndex = -1;
        private float pauseAdvanceAt = -1f;
        private bool openingWakeComplete;
        private bool doorBrushBeatComplete;
        private bool retoOpeningComplete;
        private bool bookTakenForReview;
        private bool ariaObservedForReview;
        private bool waitingForPastObservation;
        private bool pastObservationComplete;
        private bool waitingForRetoBookShow;
        private bool bookShownToRetoForReview;
        private bool currentDeskBookVisibleForReview;
        private bool retoEventComplete;
        private bool vsClear;

        public string CurrentBeatIdForReview => currentBeatId;
        public bool OpeningWakeCompleteForReview => openingWakeComplete;
        public bool DoorBrushBeatCompleteForReview => doorBrushBeatComplete;
        public int DoorBrushBeatPageForReview => doorBeatPage;
        public bool RetoOpeningCompleteForReview => retoOpeningComplete;
        public bool BookTakenForReview => bookTakenForReview;
        public bool WaitingForPastObservationForReview => waitingForPastObservation;
        public bool PastObservationCompleteForReview => pastObservationComplete;
        public bool WaitingForRetoBookShowForReview => waitingForRetoBookShow;
        public bool WaitingForPresentReturnForReview => waitingForRetoBookShow;
        public bool BookShownToRetoForReview => bookShownToRetoForReview;
        public bool CurrentDeskBookVisibleForReview => currentDeskBookVisibleForReview;
        public bool RetoEventCompleteForReview => retoEventComplete;
        public bool VsClearForReview => vsClear;
        public int RetoBeatIndexForReview => retoStepIndex;
        public bool RetoInteractionReadyForReview => IsRetoInteractionReady();
        public bool PastBookInteractionReadyForReview => IsPastBookInteractionReady();
        public bool AriaInteractionReadyForReview => IsAriaInteractionReady();
        public bool RetoBookShowReadyForReview => IsRetoBookShowReady();
        public bool PortalInputUnlockedForReview => portalController != null && portalController.RuntimeInputEnabledForReview;
        public bool CurrentTimeWindowBookCueVisibleForReview => currentTimeWindowBookCueObject != null && currentTimeWindowBookCueObject.activeSelf;
        public bool CurrentTimeWindowAriaCueVisibleForReview => currentTimeWindowAriaCueObject != null && currentTimeWindowAriaCueObject.activeSelf;
        public bool TimewriterPocketGlowVisibleForReview => timewriterPocketGlowObject != null && timewriterPocketGlowObject.activeSelf;
        public string CurrentLineTextForReview => GetCurrentDialogueTextForReview();
        public string CurrentLineSpeakerForReview => GetCurrentDialogueSpeakerForReview();
        public string ActiveRetoSequenceForReview => activeRetoSequence.ToString();
        public bool UsesRuntimeHudForReview => runtimeHud != null;
        public string RuntimeHudFontNameForReview => runtimeHud != null ? runtimeHud.FontNameForReview : string.Empty;
        public string RuntimeHudActiveTextForReview => runtimeHud != null ? runtimeHud.ActiveFullTextForReview : string.Empty;
        public string RuntimeHudVisibleTextForReview => runtimeHud != null ? runtimeHud.VisibleTextForReview : string.Empty;
        public string RuntimeHudObjectiveTextForReview => runtimeHud != null ? runtimeHud.ObjectiveTextForReview : string.Empty;
        public bool RuntimeHudQuestionActiveForReview => runtimeHud != null && runtimeHud.QuestionActiveForReview;
        public bool RuntimeHudBrushActiveForReview => runtimeHud != null && runtimeHud.BrushActiveForReview;
        public Vector2 RuntimeHudBrushAnchoredPositionForReview => runtimeHud != null ? runtimeHud.BrushAnchoredPositionForReview : Vector2.positiveInfinity;
        public string RuntimeHudBrushIconTextureNameForReview => runtimeHud != null ? runtimeHud.BrushIconTextureNameForReview : string.Empty;
        public float RuntimeHudQuestionHeadWorldOffsetForReview => runtimeHud != null ? runtimeHud.QuestionHeadWorldOffsetForReview : 0f;
        public bool UsesTmpDialoguePresenterForReview => dialoguePresenter != null && dialoguePresenter.TryEnsureForReview();
        public string DialoguePresenterFontNameForReview => dialoguePresenter != null ? dialoguePresenter.FontNameForReview : string.Empty;
        public string DialoguePresenterActiveTextForReview => dialoguePresenter != null ? dialoguePresenter.ActiveTextForReview : string.Empty;

        private void Awake()
        {
            ResolveReferences();
            lastArea = areaVisibility != null ? areaVisibility.ActiveAreaForReview : FastVsHouseArea.Interior;
            if (retoAnimator != null)
            {
                retoAnimator.SetWritingImmediateForReview();
            }

            SetCurrentDeskBookVisible(false);
            SetCurrentTimeWindowCuesVisible(false);
            SetPastTargetBookVisible(true);
            SetTimewriterPocketGlowVisible(false);
            portalController?.SetRuntimeInputEnabledForReview(false);
            CompleteOpeningWakeWithoutDialogue();
        }

        private void Update()
        {
            ResolveReferences();
            if (mode != StoryMode.None)
            {
                if (TryGetActiveModeStep(out var activeStep) && activeStep.IsPause)
                {
                    if (Time.time >= pauseAdvanceAt)
                    {
                        AdvanceStoryForReview();
                    }
                }
                else if (Time.frameCount != ignoreInputFrame && AdvancePressed())
                {
                    if (CompleteTypingBeforeAdvancing())
                    {
                        return;
                    }

                    AdvanceStoryForReview();
                }

                if (areaVisibility != null)
                {
                    lastArea = areaVisibility.ActiveAreaForReview;
                }

                return;
            }

            if (waitingForPastObservation && IsAriaInteractionReady() && AdvancePressed())
            {
                TriggerAriaObservationForReview();
            }
            else if (waitingForPastObservation && IsPastBookInteractionReady() && AdvancePressed())
            {
                TriggerPastObservationForReview();
            }
            else if (!retoEventComplete && waitingForRetoBookShow && IsRetoBookShowReady() && AdvancePressed())
            {
                TriggerRetoBookReturnForReview();
            }
            else if (!retoEventComplete && IsRetoInteractionReady() && Input.GetKeyDown(KeyCode.E))
            {
                TriggerRetoEventForReview();
            }

            if (areaVisibility != null)
            {
                lastArea = areaVisibility.ActiveAreaForReview;
            }
        }

        private void LateUpdate()
        {
            ResolveReferences();
            RefreshPresentationForReview();
        }

        private void OnGUI()
        {
            if (runtimeHud != null || UsesTmpDialoguePresenterForReview)
            {
                return;
            }

            if (mode == StoryMode.DoorBrushBeat)
            {
                if (doorBeatPage == 0)
                {
                    DrawQuestionAbovePlayer();
                }
                else if (doorBeatPage == 1)
                {
                    DrawBrushIconAtScreenCenter();
                    DrawStoryPanel("ニロ", "(ポケットに、何か...)", "▽");
                }
                else
                {
                    DrawBrushIconAtScreenCenter();
                    DrawStoryPanel("ニロ", "(...筆?)", "▽");
                }

                return;
            }

            if (mode == StoryMode.OpeningWake)
            {
                return;
            }

            if (mode == StoryMode.RetoDialogue)
            {
                if (TryGetActiveStep(out var step) && !step.IsPause)
                {
                    DrawStoryPanel(step.Speaker, step.Text, "▽");
                }

                return;
            }

            if (mode == StoryMode.AriaDialogue)
            {
                if (TryGetActiveAriaStep(out var step) && !step.IsPause)
                {
                    DrawStoryPanel(step.Speaker, step.Text, "▽");
                }

                return;
            }

            if (waitingForPastObservation)
            {
                DrawSmallObjective(ResolvePastObservationObjective());
                return;
            }

            if (waitingForRetoBookShow)
            {
                DrawSmallObjective(ResolveRetoBookShowObjective());
                return;
            }

            if (showOpeningHint && !doorBrushBeatComplete && areaVisibility != null && areaVisibility.ActiveAreaForReview == FastVsHouseArea.Interior)
            {
                DrawSmallObjective("ベッドから起きた。外へ出る。");
            }
            else if (!retoEventComplete && IsRetoInteractionReady())
            {
                DrawSmallObjective("E: レトと話す");
            }
            else if (vsClear)
            {
                DrawSmallObjective("レトの話を聞いた。");
            }
        }

        public void RefreshPresentationForReview()
        {
            if (RefreshRuntimeHudPresentation())
            {
                return;
            }

            if (dialoguePresenter == null || !dialoguePresenter.TryEnsureForReview())
            {
                return;
            }

            dialoguePresenter.SetCameraForReview(storyCamera);
            if (mode == StoryMode.OpeningWake)
            {
                dialoguePresenter.HideAll();
                return;
            }

            if (mode == StoryMode.DoorBrushBeat)
            {
                dialoguePresenter.HideAll();
                return;
            }

            if (mode == StoryMode.RetoDialogue)
            {
                if (TryGetActiveStep(out var step) && !step.IsPause)
                {
                    dialoguePresenter.ShowDialogue(step.Speaker, step.Text, "▽");
                }
                else
                {
                    dialoguePresenter.HideAll();
                }

                return;
            }

            if (mode == StoryMode.AriaDialogue)
            {
                if (TryGetActiveAriaStep(out var step) && !step.IsPause)
                {
                    dialoguePresenter.ShowDialogue(step.Speaker, step.Text, "▽");
                }
                else
                {
                    dialoguePresenter.HideAll();
                }

                return;
            }

            if (waitingForPastObservation)
            {
                dialoguePresenter.ShowObjective(ResolvePastObservationObjective());
                return;
            }

            if (waitingForRetoBookShow)
            {
                dialoguePresenter.ShowObjective(ResolveRetoBookShowObjective());
                return;
            }

            if (showOpeningHint && !doorBrushBeatComplete && areaVisibility != null && areaVisibility.ActiveAreaForReview == FastVsHouseArea.Interior)
            {
                dialoguePresenter.ShowObjective("ベッドから起きた。外へ出る。");
                return;
            }

            if (!retoEventComplete && IsRetoInteractionReady())
            {
                dialoguePresenter.ShowObjective("E: レトと話す");
                return;
            }

            if (vsClear)
            {
                dialoguePresenter.ShowObjective("レトの話を聞いた。");
                return;
            }

            dialoguePresenter.HideAll();
        }

        public void CompleteRuntimeHudTypingForReview()
        {
            runtimeHud?.CompleteTypingNow();
        }

        public void TriggerDoorBrushBeatForReview()
        {
            if (doorBrushBeatComplete)
            {
                return;
            }

            mode = StoryMode.DoorBrushBeat;
            activeRetoSequence = RetoSequence.None;
            doorBeatPage = 0;
            currentBeatId = "opening.timewriter_pocket_beat";
            ignoreInputFrame = Time.frameCount;
            FreezeMovement(true);
        }

        public bool TryBlockHouseExitForDoorBrushBeat(FastVsHouseArea sourceArea, FastVsHouseArea targetArea)
        {
            return TryBlockHouseExitForDoorBrushBeat(sourceArea, targetArea, false);
        }

        public bool TryBlockHouseExitForDoorBrushBeat(FastVsHouseArea sourceArea, FastVsHouseArea targetArea, bool forceTrigger)
        {
            if (sourceArea != FastVsHouseArea.Interior ||
                targetArea != FastVsHouseArea.Exterior ||
                doorBrushBeatComplete)
            {
                return false;
            }

            if (!openingWakeComplete)
            {
                return true;
            }

            if (mode == StoryMode.DoorBrushBeat)
            {
                return true;
            }

            if (mode != StoryMode.None)
            {
                return true;
            }

            if (!forceTrigger && !IsPlayerInsideDoorBrushBeatTrigger())
            {
                return false;
            }

            TriggerDoorBrushBeatForReview();
            return true;
        }

        public void TriggerOpeningWakeForReview()
        {
            CompleteOpeningWakeWithoutDialogue();
        }

        public void TriggerRetoEventForReview()
        {
            if (retoEventComplete)
            {
                return;
            }

            retoEventComplete = false;
            vsClear = false;
            retoOpeningComplete = false;
            bookTakenForReview = false;
            ariaObservedForReview = false;
            waitingForPastObservation = false;
            pastObservationComplete = false;
            waitingForRetoBookShow = false;
            bookShownToRetoForReview = false;
            currentDeskBookVisibleForReview = false;
            SetCurrentDeskBookVisible(false);
            SetPastTargetBookVisible(true);
            SetCurrentTimeWindowCuesVisible(false);
            SetTimewriterPocketGlowVisible(false);
            BeginRetoSequence(RetoSequence.OpeningAndActivation);
        }

        public void TriggerAriaObservationForReview()
        {
            if (!waitingForPastObservation || !IsAriaInteractionReady())
            {
                return;
            }

            mode = StoryMode.AriaDialogue;
            activeRetoSequence = RetoSequence.None;
            ariaStepIndex = 0;
            ignoreInputFrame = Time.frameCount;
            currentBeatId = AriaPastSteps[ariaStepIndex].BeatId;
            FreezeMovement(true);
            ApplyAriaStepPresentationForCurrentLine();
        }

        public void TriggerPastObservationForReview()
        {
            if (!waitingForPastObservation || !IsPastBookInteractionReady())
            {
                return;
            }

            SetCurrentTimeWindowBookCueVisible(false);
            BeginRetoSequence(RetoSequence.PastObservation);
        }

        public void TriggerRetoBookReturnForReview()
        {
            if (!waitingForRetoBookShow || retoEventComplete)
            {
                return;
            }

            waitingForRetoBookShow = false;
            bookShownToRetoForReview = true;
            SetCurrentDeskBookVisible(false);
            BeginRetoSequence(RetoSequence.ReturnAndHint);
        }

        public void TriggerPresentReturnForReview()
        {
            TriggerRetoBookReturnForReview();
        }

        public void MarkPastBookTakenForReview()
        {
            bookTakenForReview = true;
            SetCurrentDeskBookVisible(false);
            SetCurrentTimeWindowBookCueVisible(false);
            SetPastTargetBookVisible(false);
        }

        private bool RefreshRuntimeHudPresentation()
        {
            if (runtimeHud == null)
            {
                return false;
            }

            runtimeHud.SetPersistentObjective(ResolvePersistentObjectiveText());

            if (mode == StoryMode.OpeningWake)
            {
                runtimeHud.HideAll();
                return true;
            }

            if (mode == StoryMode.DoorBrushBeat)
            {
                var doorBeatText = doorBeatPage == 0
                    ? string.Empty
                    : doorBeatPage == 1
                    ? "(ポケットに、何か...)"
                        : "(...筆?)";
                runtimeHud.ShowDoorBeat(
                    player,
                    storyCamera,
                    "ニロ",
                    doorBeatText,
                    "▽",
                    doorBeatPage == 0,
                    doorBeatPage >= 1);
                return true;
            }

            if (mode == StoryMode.RetoDialogue)
            {
                if (TryGetActiveStep(out var step) && !step.IsPause)
                {
                    if (IsGuideStep(step))
                    {
                        runtimeHud.ShowGuide(step.Text, "▽");
                    }
                    else
                    {
                        runtimeHud.ShowDialogue(step.Speaker, step.Text, "▽");
                    }
                }
                else
                {
                    runtimeHud.HideAll();
                }

                return true;
            }

            if (mode == StoryMode.AriaDialogue)
            {
                if (TryGetActiveAriaStep(out var step) && !step.IsPause)
                {
                    runtimeHud.ShowDialogue(step.Speaker, step.Text, "▽");
                }
                else
                {
                    runtimeHud.HideAll();
                }

                return true;
            }

            if (waitingForPastObservation)
            {
                runtimeHud.ShowObjective(ResolvePastObservationObjective());
                return true;
            }

            if (waitingForRetoBookShow)
            {
                runtimeHud.ShowObjective(ResolveRetoBookShowObjective());
                return true;
            }

            if (showOpeningHint && !doorBrushBeatComplete && areaVisibility != null && areaVisibility.ActiveAreaForReview == FastVsHouseArea.Interior)
            {
                runtimeHud.ShowObjective("ベッドから起きた。外へ出る。");
                return true;
            }

            if (!retoEventComplete && IsRetoInteractionReady())
            {
                runtimeHud.ShowObjective("E: レトと話す");
                return true;
            }

            if (vsClear)
            {
                runtimeHud.ShowObjective("レトの話を聞いた。");
                return true;
            }

            runtimeHud.HideAll();
            return true;
        }

        public void AdvanceStoryForReview()
        {
            if (mode == StoryMode.DoorBrushBeat)
            {
                doorBeatPage++;
                if (doorBeatPage <= 2)
                {
                    return;
                }

                doorBrushBeatComplete = true;
                mode = StoryMode.None;
                currentBeatId = "route.house_exterior";
                FreezeMovement(false);
                return;
            }

            if (mode == StoryMode.OpeningWake)
            {
                CompleteOpeningWakeWithoutDialogue();
                return;
            }

            if (mode == StoryMode.AriaDialogue)
            {
                ariaStepIndex++;
                if (ariaStepIndex >= AriaPastSteps.Length)
                {
                    CompleteAriaObservation();
                    return;
                }

                currentBeatId = AriaPastSteps[ariaStepIndex].BeatId;
                ApplyAriaStepPresentationForCurrentLine();
                return;
            }

            if (mode != StoryMode.RetoDialogue)
            {
                return;
            }

            retoStepIndex++;
            if (retoStepIndex >= GetActiveRetoSteps().Length)
            {
                CompleteActiveRetoSequence();
                return;
            }

            currentBeatId = GetActiveRetoSteps()[retoStepIndex].BeatId;
            ApplyRetoStepPresentationForCurrentLine();
        }

        private void BeginRetoSequence(RetoSequence sequence)
        {
            var steps = GetActiveRetoSteps(sequence);
            if (steps.Length == 0)
            {
                return;
            }

            activeRetoSequence = sequence;
            mode = StoryMode.RetoDialogue;
            retoStepIndex = 0;
            ignoreInputFrame = Time.frameCount;
            currentBeatId = steps[retoStepIndex].BeatId;
            FreezeMovement(true);
            SetTimewriterPocketGlowVisible(false);
            ApplyRetoStepPresentationForCurrentLine();
        }

        private void CompleteActiveRetoSequence()
        {
            mode = StoryMode.None;
            pauseAdvanceAt = -1f;
            SetTimewriterPocketGlowVisible(false);

            switch (activeRetoSequence)
            {
                case RetoSequence.OpeningAndActivation:
                    retoOpeningComplete = true;
                    waitingForPastObservation = true;
                    currentBeatId = "scene1.reto.1e.await_past_library_observation";
                    portalController?.SetRuntimeInputEnabledForReview(true);
                    SetCurrentTimeWindowCuesVisible(true);
                    FreezeMovement(false);
                    if (retoAnimator != null)
                    {
                        retoAnimator.SetWritingForReview();
                    }

                    return;

                case RetoSequence.PastObservation:
                    FreezeMovement(false);
                    TryCompletePastObservationGate();
                    return;

                case RetoSequence.ReturnAndHint:
                    CompleteRetoEvent();
                    return;
            }
        }

        private void CompleteRetoEvent()
        {
            activeRetoSequence = RetoSequence.None;
            mode = StoryMode.None;
            retoEventComplete = true;
            vsClear = true;
            waitingForRetoBookShow = false;
            bookShownToRetoForReview = true;
            SetCurrentDeskBookVisible(true);
            SetCurrentTimeWindowCuesVisible(false);
            SetTimewriterPocketGlowVisible(false);
            currentBeatId = "vs.clear";
            pauseAdvanceAt = -1f;
            FreezeMovement(false);
            if (retoAnimator != null)
            {
                retoAnimator.SetWritingForReview();
            }
        }

        private bool CompleteTypingBeforeAdvancing()
        {
            if (runtimeHud == null || !runtimeHud.IsTyping)
            {
                return false;
            }

            runtimeHud.CompleteTypingNow();
            return true;
        }

        private void CompleteOpeningWakeWithoutDialogue()
        {
            openingWakeComplete = true;
            mode = StoryMode.None;
            activeRetoSequence = RetoSequence.None;
            currentBeatId = "opening.house_interior";
            ignoreInputFrame = -1;
            FreezeMovement(false);
        }

        private static bool IsGuideStep(StoryStep step)
        {
            return step.Speaker == "案内";
        }

        private string ResolvePersistentObjectiveText()
        {
            if (mode != StoryMode.None)
            {
                return string.Empty;
            }
            if (!openingWakeComplete)
            {
                return "ベッドから起きる。";
            }

            if (mode == StoryMode.DoorBrushBeat)
            {
                return string.Empty;
            }

            if (waitingForPastObservation)
            {
                return ResolvePastObservationObjective();
            }

            if (waitingForRetoBookShow)
            {
                return ResolveRetoBookShowObjective();
            }

            if (vsClear)
            {
                return "レトの話を聞いた。";
            }

            if (areaVisibility == null)
            {
                return "外へ出る。";
            }

            switch (areaVisibility.ActiveAreaForReview)
            {
                case FastVsHouseArea.Interior:
                    return doorBrushBeatComplete ? "外へ出る。" : "外へ出る。";
                case FastVsHouseArea.Exterior:
                    return "北東の道を進む。";
                case FastVsHouseArea.CentralPlaza:
                    return "図書館へ向かう。";
                case FastVsHouseArea.Library:
                    return !retoEventComplete && IsRetoInteractionReady()
                        ? "E: レトと話す"
                        : "レトの机へ向かう。";
                default:
                    return "進む。";
            }
        }

        private string ResolvePastObservationObjective()
        {
            if (IsAriaInteractionReady())
            {
                return "E: 過去の人影を見る";
            }

            if (IsPastBookInteractionReady())
            {
                return "E: 光っている本を調べる";
            }

            if (areaVisibility != null &&
                portalController != null &&
                areaVisibility.ActiveAreaForReview == FastVsHouseArea.Library &&
                portalController.PlayerInOtherTime)
            {
                return "過去の図書館で、光る本か人影の近くへ行く。";
            }

            return "黄色い光の近くに、左ドラッグで時の窓を開く。";
        }

        private string ResolveRetoBookShowObjective()
        {
            if (IsRetoBookShowReady())
            {
                return "E: レトに本を見せる";
            }

            if (areaVisibility != null &&
                portalController != null &&
                areaVisibility.ActiveAreaForReview == FastVsHouseArea.Library &&
                portalController.PlayerInOtherTime)
            {
                return "時の窓から、現在の図書館へ戻る。";
            }

            if (areaVisibility != null && areaVisibility.ActiveAreaForReview == FastVsHouseArea.Library)
            {
                return "レトの机へ戻る。";
            }

            return "図書館でレトの机へ戻る。";
        }

        private void ApplyRetoStepPresentationForCurrentLine()
        {
            if (!TryGetActiveStep(out var step))
            {
                return;
            }

            if (step.BeatId == "scene1.reto.1e.past_library_observation.take_book")
            {
                MarkPastBookTakenForReview();
            }

            if (step.BeatId == "scene1.reto.1f.return_present.show_book")
            {
                bookShownToRetoForReview = true;
                SetCurrentDeskBookVisible(false);
            }

            ApplyRetoPoseForCurrentStep(step);
            SetTimewriterPocketGlowVisible(IsTimewriterPocketGlowStep(step));

            if (step.IsPause)
            {
                pauseAdvanceAt = Time.time + step.PauseSeconds;
            }
            else
            {
                pauseAdvanceAt = -1f;
            }
        }

        private void ApplyAriaStepPresentationForCurrentLine()
        {
            if (!TryGetActiveAriaStep(out var step))
            {
                return;
            }

            if (step.IsPause)
            {
                pauseAdvanceAt = Time.time + step.PauseSeconds;
            }
            else
            {
                pauseAdvanceAt = -1f;
            }
        }

        private void CompleteAriaObservation()
        {
            mode = StoryMode.None;
            ariaStepIndex = -1;
            pauseAdvanceAt = -1f;
            ariaObservedForReview = true;
            SetCurrentTimeWindowAriaCueVisible(false);
            FreezeMovement(false);
            TryCompletePastObservationGate();
        }

        private void ApplyRetoPoseForCurrentStep(StoryStep step)
        {
            if (retoAnimator == null)
            {
                return;
            }

            if (step.PoseState == FastVsRetoWritingState.LookingUp)
            {
                retoAnimator.SetLookingUpForReview();
                return;
            }

            if (step.PoseState == FastVsRetoWritingState.Lowering)
            {
                retoAnimator.SetLoweringForReview();
                return;
            }

            if (step.PoseState == FastVsRetoWritingState.Raising)
            {
                retoAnimator.SetRaisingForReview();
                return;
            }

            if (step.PoseState == FastVsRetoWritingState.WritingRaised || step.BeatId == "vs.clear")
            {
                retoAnimator.SetWritingForReview();
                return;
            }

            retoAnimator.SetDialogueForReview();
        }

        private bool IsRetoInteractionReady()
        {
            if (waitingForPastObservation ||
                waitingForRetoBookShow ||
                areaVisibility == null ||
                areaVisibility.ActiveAreaForReview != FastVsHouseArea.Library ||
                portalController == null ||
                player == null ||
                portalController.PlayerInOtherTime)
            {
                return false;
            }

            return IsWithinRetoInteractionRadius();
        }

        private bool IsPastBookInteractionReady()
        {
            if (!waitingForPastObservation ||
                bookTakenForReview ||
                areaVisibility == null ||
                areaVisibility.ActiveAreaForReview != FastVsHouseArea.Library ||
                portalController == null ||
                player == null ||
                !portalController.PlayerInOtherTime)
            {
                return false;
            }

            var local = portalController.OtherTimeSpaceRootForReview != null
                ? portalController.OtherTimeSpaceRootForReview.InverseTransformPoint(player.position)
                : player.position;
            local.y = pastLibraryBookLocalPosition.y;
            return Vector3.Distance(local, pastLibraryBookLocalPosition) <= pastBookInteractionRadius;
        }

        private bool IsAriaInteractionReady()
        {
            if (!waitingForPastObservation ||
                ariaObservedForReview ||
                areaVisibility == null ||
                areaVisibility.ActiveAreaForReview != FastVsHouseArea.Library ||
                portalController == null ||
                player == null ||
                !portalController.PlayerInOtherTime)
            {
                return false;
            }

            var local = portalController.OtherTimeSpaceRootForReview != null
                ? portalController.OtherTimeSpaceRootForReview.InverseTransformPoint(player.position)
                : player.position;
            local.y = pastLibraryAriaLocalPosition.y;
            return Vector3.Distance(local, pastLibraryAriaLocalPosition) <= pastAriaInteractionRadius;
        }

        private bool IsRetoBookShowReady()
        {
            if (!waitingForRetoBookShow ||
                areaVisibility == null ||
                areaVisibility.ActiveAreaForReview != FastVsHouseArea.Library ||
                portalController == null ||
                player == null ||
                portalController.PlayerInOtherTime)
            {
                return false;
            }

            return IsWithinRetoInteractionRadius();
        }

        private bool IsWithinRetoInteractionRadius()
        {
            var local = portalController.CurrentSpaceRootForReview != null
                ? portalController.CurrentSpaceRootForReview.InverseTransformPoint(player.position)
                : player.position;
            local.y = retoLocalPosition.y;
            return Vector3.Distance(local, retoLocalPosition) <= retoInteractionRadius;
        }

        private bool IsPlayerInsideDoorBrushBeatTrigger()
        {
            if (areaVisibility == null ||
                areaVisibility.ActiveAreaForReview != FastVsHouseArea.Interior ||
                portalController == null ||
                player == null ||
                portalController.PlayerInOtherTime ||
                portalController.CurrentSpaceRootForReview == null)
            {
                return false;
            }

            var local = portalController.CurrentSpaceRootForReview.InverseTransformPoint(player.position);
            return Contains(local, doorBrushBeatTriggerLocalCenter, doorBrushBeatTriggerLocalSize);
        }

        private static bool Contains(Vector3 point, Vector3 center, Vector3 size)
        {
            var half = size * 0.5f;
            return Mathf.Abs(point.x - center.x) <= half.x &&
                   Mathf.Abs(point.y - center.y) <= half.y &&
                   Mathf.Abs(point.z - center.z) <= half.z;
        }

        private bool TryGetActiveStep(out StoryStep step)
        {
            var steps = GetActiveRetoSteps();
            if (mode == StoryMode.RetoDialogue && retoStepIndex >= 0 && retoStepIndex < steps.Length)
            {
                step = steps[retoStepIndex];
                return true;
            }

            step = default;
            return false;
        }

        private bool TryGetActiveAriaStep(out StoryStep step)
        {
            if (mode == StoryMode.AriaDialogue && ariaStepIndex >= 0 && ariaStepIndex < AriaPastSteps.Length)
            {
                step = AriaPastSteps[ariaStepIndex];
                return true;
            }

            step = default;
            return false;
        }

        private bool TryGetActiveModeStep(out StoryStep step)
        {
            if (TryGetActiveStep(out step))
            {
                return true;
            }

            return TryGetActiveAriaStep(out step);
        }

        private bool TryGetActiveDialogueStep(out StoryStep step)
        {
            if (TryGetActiveModeStep(out step) && !step.IsPause)
            {
                return true;
            }

            step = default;
            return false;
        }

        private string GetCurrentDialogueTextForReview()
        {
            return TryGetActiveDialogueStep(out var step) ? step.Text : string.Empty;
        }

        private string GetCurrentDialogueSpeakerForReview()
        {
            return TryGetActiveDialogueStep(out var step) ? step.Speaker : string.Empty;
        }

        private static bool IsTimewriterPocketGlowStep(StoryStep step)
        {
            return step.BeatId == "scene1.reto.1d.timewriter_activation.pocket_glow_pause" ||
                   step.BeatId == "scene1.reto.1d.timewriter_activation.pause_after_pocket" ||
                   step.Text == "(筆が...!)";
        }

        private StoryStep[] GetActiveRetoSteps()
        {
            return GetActiveRetoSteps(activeRetoSequence);
        }

        private StoryStep[] GetActiveRetoSteps(RetoSequence sequence)
        {
            switch (sequence)
            {
                case RetoSequence.OpeningAndActivation:
                    return RetoOpeningSteps;
                case RetoSequence.PastObservation:
                    return RetoPastObservationSteps;
                case RetoSequence.ReturnAndHint:
                    return RetoReturnAndHintSteps;
                default:
                    return System.Array.Empty<StoryStep>();
            }
        }

        private void FreezeMovement(bool frozen)
        {
            if (movementGuide != null)
            {
                movementGuide.SetMovementFrozen(frozen);
            }
        }

        private static bool AdvancePressed()
        {
            return Input.GetKeyDown(KeyCode.Space) ||
                   Input.GetKeyDown(KeyCode.Return) ||
                   Input.GetKeyDown(KeyCode.E);
        }

        private void ResolveReferences()
        {
            if (portalController == null)
            {
                portalController = FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            }

            if (areaVisibility == null)
            {
                areaVisibility = FindFirstObjectByType<FastVsHouseAreaVisibility>();
            }

            if (movementGuide == null)
            {
                movementGuide = FindFirstObjectByType<FastVsVisualDirectionGuide>();
            }

            if (playerController == null)
            {
                playerController = FindFirstObjectByType<CharacterController>();
            }

            if (player == null && playerController != null)
            {
                player = playerController.transform;
            }

            if (storyCamera == null)
            {
                storyCamera = Camera.main;
            }

            if (retoAnimator == null)
            {
                retoAnimator = FindFirstObjectByType<FastVsRetoWritingAnimator>();
            }

            if (dialoguePresenter == null)
            {
                dialoguePresenter = FindFirstObjectByType<FastVsStoryDialoguePresenter>();
            }

            if (runtimeHud == null)
            {
                runtimeHud = FindFirstObjectByType<FastVsStoryRuntimeHud>();
            }
        }

        private void SetCurrentDeskBookVisible(bool visible)
        {
            currentDeskBookVisibleForReview = visible;
            if (currentDeskBookObject != null && currentDeskBookObject.activeSelf != visible)
            {
                currentDeskBookObject.SetActive(visible);
            }
        }

        private void SetCurrentTimeWindowBookCueVisible(bool visible)
        {
            if (currentTimeWindowBookCueObject != null && currentTimeWindowBookCueObject.activeSelf != visible)
            {
                currentTimeWindowBookCueObject.SetActive(visible);
            }
        }

        private void SetCurrentTimeWindowAriaCueVisible(bool visible)
        {
            if (currentTimeWindowAriaCueObject != null && currentTimeWindowAriaCueObject.activeSelf != visible)
            {
                currentTimeWindowAriaCueObject.SetActive(visible);
            }

            if (pastAriaMarkerObject != null && pastAriaMarkerObject.activeSelf != visible)
            {
                pastAriaMarkerObject.SetActive(visible);
            }
        }

        private void SetTimewriterPocketGlowVisible(bool visible)
        {
            if (timewriterPocketGlowObject != null && timewriterPocketGlowObject.activeSelf != visible)
            {
                timewriterPocketGlowObject.SetActive(visible);
            }
        }

        private void SetPastTargetBookVisible(bool visible)
        {
            if (pastTargetBookObject != null && pastTargetBookObject.activeSelf != visible)
            {
                pastTargetBookObject.SetActive(visible);
            }

            if (pastTargetBookMarkerObject != null && pastTargetBookMarkerObject.activeSelf != visible)
            {
                pastTargetBookMarkerObject.SetActive(visible);
            }
        }

        private void SetCurrentTimeWindowCuesVisible(bool visible)
        {
            SetCurrentTimeWindowBookCueVisible(visible && !bookTakenForReview);
            SetCurrentTimeWindowAriaCueVisible(visible && !ariaObservedForReview);
        }

        private void TryCompletePastObservationGate()
        {
            if (!waitingForPastObservation)
            {
                return;
            }

            if (!bookTakenForReview || !ariaObservedForReview)
            {
                currentBeatId = "scene1.reto.1e.await_past_library_observation";
                SetCurrentTimeWindowCuesVisible(true);
                return;
            }

            waitingForPastObservation = false;
            pastObservationComplete = true;
            waitingForRetoBookShow = true;
            currentBeatId = "scene1.reto.1f.await_reto_book_show";
            SetCurrentDeskBookVisible(false);
            SetCurrentTimeWindowCuesVisible(false);
            if (retoAnimator != null)
            {
                retoAnimator.SetWritingForReview();
            }
        }

        private void DrawQuestionAbovePlayer()
        {
            if (storyCamera == null || player == null)
            {
                return;
            }

            var screen = storyCamera.WorldToScreenPoint(player.position + Vector3.up * QuestionHeadWorldOffset);
            if (screen.z <= 0f)
            {
                return;
            }

            var rect = new Rect(screen.x - 18f, Screen.height - screen.y - 46f, 36f, 36f);
            var oldColor = GUI.color;
            GUI.color = new Color(1f, 0.93f, 0.52f, 1f);
            GUI.Label(rect, "?", QuestionStyle());
            GUI.color = oldColor;
        }

        private static void DrawBrushIconAtScreenCenter()
        {
            var size = Mathf.Min(Screen.width, Screen.height) * 0.16f;
            var center = new Vector2(Screen.width * 0.5f, Screen.height * 0.42f);
            var frame = new Rect(center.x - size * 0.58f, center.y - size * 0.58f, size * 1.16f, size * 1.16f);
            var inner = new Rect(frame.x + 10f, frame.y + 10f, frame.width - 20f, frame.height - 20f);
            DrawRect(frame, new Color(0.22f, 0.17f, 0.13f, 0.95f));
            DrawRect(inner, new Color(0.06f, 0.055f, 0.05f, 0.95f));
            DrawRect(new Rect(inner.x + inner.width * 0.5f - 2f, inner.y + 14f, 4f, inner.height - 28f), new Color(0.74f, 0.58f, 0.38f, 0.78f));
            DrawRect(new Rect(inner.x + 14f, inner.y + inner.height * 0.5f - 2f, inner.width - 28f, 4f), new Color(0.74f, 0.58f, 0.38f, 0.78f));
            DrawRect(new Rect(center.x - size * 0.10f, center.y - size * 0.44f, size * 0.20f, size * 0.66f), new Color(0.52f, 0.28f, 0.13f, 1f));
            DrawRect(new Rect(center.x - size * 0.17f, center.y + size * 0.18f, size * 0.34f, size * 0.12f), new Color(0.88f, 0.70f, 0.38f, 1f));
            DrawRect(new Rect(center.x - size * 0.11f, center.y + size * 0.29f, size * 0.22f, size * 0.23f), new Color(0.12f, 0.08f, 0.06f, 1f));
        }

        private static void DrawStoryPanel(string speaker, string text, string advance)
        {
            var panel = new Rect(54f, Screen.height - 168f, Screen.width - 108f, 124f);
            DrawRect(panel, new Color(0.025f, 0.022f, 0.022f, 0.88f));
            DrawRect(new Rect(panel.x, panel.y, panel.width, 3f), new Color(0.95f, 0.57f, 0.26f, 0.92f));
            GUI.Label(new Rect(panel.x + 22f, panel.y + 14f, 240f, 30f), speaker, SpeakerStyle());
            GUI.Label(new Rect(panel.x + 22f, panel.y + 48f, panel.width - 44f, 46f), text, DialogueStyle());
            GUI.Label(new Rect(panel.x + panel.width - 190f, panel.y + 92f, 170f, 24f), advance, AdvanceStyle());
        }

        private static void DrawSmallObjective(string text)
        {
            var panel = new Rect(18f, Screen.height - 58f, 520f, 40f);
            DrawRect(panel, new Color(0.025f, 0.022f, 0.022f, 0.72f));
            GUI.Label(new Rect(panel.x + 14f, panel.y + 8f, panel.width - 24f, 24f), text, ObjectiveStyle());
        }

        private static void DrawRect(Rect rect, Color color)
        {
            var oldColor = GUI.color;
            GUI.color = color;
            GUI.DrawTexture(rect, Texture2D.whiteTexture);
            GUI.color = oldColor;
        }

        private static GUIStyle QuestionStyle()
        {
            return new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 30,
                fontStyle = FontStyle.Bold,
                normal = { textColor = new Color(1f, 0.93f, 0.52f, 1f) }
            };
        }

        private static GUIStyle SpeakerStyle()
        {
            return new GUIStyle(GUI.skin.label)
            {
                fontSize = 22,
                fontStyle = FontStyle.Bold,
                normal = { textColor = new Color(1f, 0.80f, 0.48f, 1f) }
            };
        }

        private static GUIStyle DialogueStyle()
        {
            return new GUIStyle(GUI.skin.label)
            {
                fontSize = 24,
                wordWrap = true,
                normal = { textColor = new Color(0.96f, 0.94f, 0.88f, 1f) }
            };
        }

        private static GUIStyle AdvanceStyle()
        {
            return new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleRight,
                fontSize = 15,
                normal = { textColor = new Color(0.80f, 0.77f, 0.68f, 1f) }
            };
        }

        private static GUIStyle ObjectiveStyle()
        {
            return new GUIStyle(GUI.skin.label)
            {
                fontSize = 16,
                normal = { textColor = new Color(0.94f, 0.91f, 0.84f, 1f) }
            };
        }
    }
}
