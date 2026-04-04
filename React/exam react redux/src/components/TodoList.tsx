
import { useAppSelector } from "../hooks";
import TodoItem from "./TodoItem";

function TodoList() {
  const { todos, filter } = useAppSelector(state => state.todos);

  const filtered = todos.filter(t => {
    if (filter === "active") return !t.completed;
    if (filter === "completed") return t.completed;
    return true;
  });

  return (
    <div>
      {filtered.map(todo => (
        <TodoItem key={todo.id} todo={todo} />
      ))}
    </div>
  );
}

export default TodoList;
