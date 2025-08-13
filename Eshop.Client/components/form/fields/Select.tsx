import React from "react";
import { useFormContext, Path, FieldValues, Controller } from "react-hook-form";
import {
  Select as MuiSelect,
  MenuItem,
  FormControl,
  InputLabel,
  FormHelperText,
} from "@mui/material";

type SelectProps<T extends FieldValues> = {
  name: Path<T>;
  label?: string;
  options: { value: any; label: string }[];
} & Omit<
  React.ComponentProps<typeof MuiSelect>,
  "name" | "defaultValue" | "onChange" | "value"
>;

function Select<T extends FieldValues>({
  name,
  label,
  options,
  ...rest
}: SelectProps<T>) {
  const {
    control,
    formState: { errors },
  } = useFormContext<T>();

  const errorMessage = errors[name]?.message as string | undefined;

  return (
    <FormControl fullWidth error={!!errorMessage}>
      {label && <InputLabel>{label}</InputLabel>}
      <Controller
        name={name}
        control={control}
        render={({ field }) => (
          <MuiSelect {...field} size="small" {...rest}>
            {options.map((opt) => (
              <MenuItem key={opt.value} value={opt.value}>
                {opt.label}
              </MenuItem>
            ))}
          </MuiSelect>
        )}
      />
      {errorMessage && <FormHelperText>{errorMessage}</FormHelperText>}
    </FormControl>
  );
}

export default Select;
