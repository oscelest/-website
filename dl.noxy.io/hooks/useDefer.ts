import {useState} from "react";

export function useDefer() {
  const [timer, setTimer] = useState<number>();
  
  return <TArgs extends any[]>(callback: (...args: TArgs) => void, ms?: number, ...args: TArgs) => {
    if (timer) clearTimeout(timer);
    setTimer(window.setTimeout(callback, ms, ...args));
  };
}
