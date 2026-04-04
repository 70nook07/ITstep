
import { createSlice, PayloadAction } from "@reduxjs/toolkit";

export type Todo = {
  id: number;
  text: string;
  completed: boolean;
};

type FilterType = "all" | "active" | "completed";

type TodosState = {
  todos: Todo[];
  filter: FilterType;
};

const initialState: TodosState = {
  todos: [],
  filter: "all",
};

const todosSlice = createSlice({
  name: "todos",
  initialState,
  reducers: {
    addTodo(state, action: PayloadAction<string>) {
      state.todos.push({
        id: Date.now(),
        text: action.payload,
        completed: false,
      });
    },
    deleteTodo(state, action: PayloadAction<number>) {
      state.todos = state.todos.filter(t => t.id !== action.payload);
    },
    toggleTodo(state, action: PayloadAction<number>) {
      const todo = state.todos.find(t => t.id === action.payload);
      if (todo) todo.completed = !todo.completed;
    },
    setFilter(state, action: PayloadAction<FilterType>) {
      state.filter = action.payload;
    },
    clearCompleted(state) {
      state.todos = state.todos.filter(t => !t.completed);
    }
  },
});

export const {
  addTodo,
  deleteTodo,
  toggleTodo,
  setFilter,
  clearCompleted,
} = todosSlice.actions;

export default todosSlice.reducer;
