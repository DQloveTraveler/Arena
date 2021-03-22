using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


/// <summary>
/// �^�X�N�Ƃ����P�ʂŏ�����o�^���A
/// TaskType��ǉ����邱�Ƃŏ��Ԃɏ�������N���X
/// ���g�p��
/// using System;
/// using UnityEngine;
/// 
/// class Test : MonoBehaviour
/// {
///		// �@ enum �� �^�X�N���`����
///		enum TaskEnum { Jump, Charge, Tackle };
///		// �A �^�X�N���X�g�̐���
///		TaskList<TaskEnum> _TaskList = new TaskList<TaskEnum>();
///		void Start()
///		{
///			// �B �^�X�N�̓o�^
///			DefineTask();
///		}
///		
///		void Update()
///		{
///			// �^�X�N���I��������A���̃^�X�N��o�^����
///			if(_TaskList.IsEnd)
///			{
///				// isPlayerFar = IsPlayerFar() // �v���C���[�̋����������� true
///				if (isPlayerFar)
///				{
///					// �v���C���[�̋������������̓`���[�W���ă^�b�N��
///					_TaskList.AddTask(TaskEnum.Charge);
///					_TaskList.AddTask(TaskEnum.Tackle);
///				}
///				else
///				{
///					// �v���C���[�̋������߂����̓W�����v
///					_TaskList.AddTask(TaskEnum.Jump);
///				}
///			}
///			
///			// �^�X�N�̍X�V
///			_TaskList.UpdateTask();
///		}
/// 
///		void DefineTask()
///		{
///			�C�^�X�N�̓o�^
///			_TaskList.DefineTask(TaskEnum.Jump, OnTaskJumpEnter, OnTaskJumpUpdate, OnTaskJumpExit);
///			_TaskList.DefineTask(TaskEnum.Charge, OnTaskChargeEnter, OnTaskChargeUpdate, OnTaskChargeExit);
///			_TaskList.DefineTask(TaskEnum.Tackle, OnTaskTackleEnter, OnTaskTackleUpdate, OnTaskTackleExit);
///		}
///		
///		void OnTaskJumpEnter()
///		{
///			// Jump�^�X�N �ύX����1��Ă΂��
///		}
///		
///		bool OnTaskJumpUpdate()
///		{
///			// Jump�^�X�N ���t���[���Ă΂��
///			// (�߂�l�� true �̎��� �^�X�N���I������
///			return true;
///		}
///		
///		void OnTaskJumpExit()
///		{
///			// Jump�^�X�N �I������1��Ă΂��
///		}
///		
///		...
/// }
/// </summary>
/// <typeparam name="T"></typeparam>
public class TaskList<T>
{
	#region define
	private class Task
	{
		public T TaskType;
		public Action Enter { get; set; }
		public Func<bool> Update { get; set; }
		public Action Exit { get; set; }

		public Task(T t, Action enter, Func<bool> update, Action exit)
		{
			TaskType = t;
			Enter = enter;
			Update = update ?? delegate { return true; };
			Exit = exit;
		}
	}
	#endregion

	#region field
	/// <summary> ��`���ꂽ�^�X�N </summary>
	private Dictionary<T, Task> _defineTaskDictionary = new Dictionary<T, Task>();
	/// <summary> ���ݐς܂�Ă���^�X�N </summary>
	private List<Task> _currentTaskList = new List<Task>();
	/// <summary> ���ݓ��삵�Ă���^�X�N </summary>
	private Task _currentTask = null;
	/// <summary> ���݂�Index�ԍ� </summary>
	private int _currentIndex = 0;
	#endregion

	#region property
	/// <summary>
	/// �ǉ����ꂽ�^�X�N�����ׂďI�����Ă��邩
	/// </summary>
	public bool IsEnd => _currentTaskList.Count <= _currentIndex;

	/// <summary>
	///  �^�X�N�������Ă��邩
	/// </summary>
	public bool IsMoveTask => _currentTask != null;

	/// <summary>
	/// ���݂̃^�X�N�^�C�v
	/// </summary>
	public T CurrentTaskType
	{
		get
		{
			if (_currentTask == null)
			{
				return default(T);
			}
			return _currentTask.TaskType;
		}
	}

	/// <summary>
	/// �ǉ�����Ă���^�X�N�̃��X�g
	/// </summary>
	public List<T> CurrentTaskTypeList => _currentTaskList.Select(x => x.TaskType).ToList();

	/// <summary>
	/// ���݂̃C���f�b�N�X
	/// </summary>
	public int CurrentIndex => _currentIndex;
	#endregion

	#region public function
	/// <summary>
	/// ���t���[���Ă΂�鏈��
	/// (Behaviour��Update�ŌĂ΂��z��)
	/// </summary>
	public void UpdateTask()
	{
		// �^�X�N���Ȃ���Ή������Ȃ�
		if (IsEnd)
		{
			return;
		}

		// ���݂̃^�X�N���Ȃ���΁A�^�X�N���擾����
		if (_currentTask == null)
		{
			_currentTask = _currentTaskList[_currentIndex];
			// Enter���Ă�
			_currentTask.Enter?.Invoke();
		}

		// �^�X�N��Update���Ă�
		var isEndOneTask = _currentTask.Update();

		// �^�X�N���I�����Ă���Ύ��̏������Ă�
		if (isEndOneTask)
		{
			// ���݂̃^�X�N��Exit���Ă�
			_currentTask?.Exit();

			// Index�ǉ�
			_currentIndex++;

			// �^�X�N���Ȃ���΃N���A����
			if (IsEnd)
			{
				_currentIndex = 0;
				_currentTask = null;
				_currentTaskList.Clear();
				return;
			}

			// ���̃^�X�N���擾����
			_currentTask = _currentTaskList[_currentIndex];
			// ���̃^�X�N��Enter���Ă�
			_currentTask?.Enter();
		}
	}

	/// <summary>
	/// �^�X�N�̒�`
	/// </summary>
	/// <param name="t"></param>
	/// <param name="enter"></param>
	/// <param name="update"></param>
	/// <param name="exit"></param>
	public void DefineTask(T t, Action enter, Func<bool> update, Action exit)
	{
		var task = new Task(t, enter, update, exit);
		var exist = _defineTaskDictionary.ContainsKey(t);
		if (exist)
		{
#if UNITY_EDITOR
			Debug.LogError(GetType() + "{0}�͊��ɒǉ�����Ă��܂��B(�o�^����܂���ł���)." + t);
#endif
			return;
		}
		_defineTaskDictionary.Add(t, task);
	}

	/// <summary>
	/// �^�X�N�̓o�^
	/// </summary>
	/// <param name="t"></param>
	public void AddTask(T t)
	{
		Task task = null;
		var exist = _defineTaskDictionary.TryGetValue(t, out task);
		if (exist == false)
		{
#if UNITY_EDITOR
			Debug.LogError(GetType() + "{0}�̃^�X�N���o�^����Ă��Ȃ��̂Œǉ��ł��܂���." + t);
#endif
			return;
		}
		_currentTaskList.Add(task);
	}

	/// <summary>
	/// �����I��
	/// </summary>
	public void ForceStop()
	{
		if (_currentTask != null)
		{
			_currentTask.Exit();
		}
		_currentTask = null;
		_currentTaskList.Clear();
		_currentIndex = 0;
	}
	#endregion
}
