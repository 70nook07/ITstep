
import { useState } from "react";
import { useAppDispatch, useAppSelector } from "./hooks";
import { addTodo, clearCompleted } from "./features/todos/todosSlice";
import "./App.css";
import TodoList from "./components/TodoList";
import Filter from "./components/Filter";

function App() {
  const [text, setText] = useState("");
  const dispatch = useAppDispatch();

  const activeCount = useAppSelector(
    state => state.todos.todos.filter(t => !t.completed).length
  );

  const handleAdd = () => {
    if (!text.trim()) return;
    dispatch(addTodo(text));
    setText("");
  };

  return (
    <div className="app">
      <h1>Todo Toolkit</h1>
      <input value={text} onChange={(e) => setText(e.target.value)} />
      <button onClick={handleAdd}>Add</button>
      <Filter />
      <TodoList />
      <p>Active: {activeCount}</p>
      <button onClick={() => dispatch(clearCompleted())}>
        Clear Complete
      </button>
    </div>
  );
}

export default App;
