#include <iostream>
#include <Windows.h>

int main()
{
  HMODULE module = GetModuleHandle(NULL);
  if (module == INVALID_HANDLE_VALUE)
  {
    return 0;
  }
  return 1;
}