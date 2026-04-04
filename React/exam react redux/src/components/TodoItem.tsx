
import { Todo, deleteTodo, toggleTodo } from "../features/todos/todosSlice";
import { useAppDispatch } from "../hooks";

type Props = {
  todo: Todo;
};

function TodoItem({ todo }: Props) {
  const dispatch = useAppDispatch();

  return (
    <div className="todo">
      <input
        type="checkbox"
        checked={todo.completed}
        onChange={() => dispatch(toggleTodo(todo.id))}
      />
      <span style={{ textDecoration: todo.completed ? "line-through" : "" }}>
        {todo.text}
      </span>
      <button onClick={() => dispatch(deleteTodo(todo.id))}>X</button>
    </div>
  );
}

export default TodoItem;
