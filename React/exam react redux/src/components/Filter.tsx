
import { useAppDispatch } from "../hooks";
import { setFilter } from "../features/todos/todosSlice";

function Filter() {
  const dispatch = useAppDispatch();

  return (
    <div>
      <button onClick={() => dispatch(setFilter("all"))}>All</button>
      <button onClick={() => dispatch(setFilter("active"))}>Active</button>
      <button onClick={() => dispatch(setFilter("completed"))}>Complete</button>
    </div>
  );
}

export default Filter;
