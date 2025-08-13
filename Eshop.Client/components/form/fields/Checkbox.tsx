import React from "react";
import { useFormContext, Path, FieldValues, Controller } from "react-hook-form";
import {
  Checkbox as MuiCheckbox,
  FormControlLabel,
  FormHelperText,
  FormGroup,
} from "@mui/material";

type CheckboxProps<T extends FieldValues> = {
  name: Path<T>;
  label?: string;
} & Omit<
  React.ComponentProps<typeof MuiCheckbox>,
  "name" | "defaultValue" | "onChange" | "value"
>;
function Checkbox<T extends FieldValues>({
  name,
  label,
  ...rest
}: CheckboxProps<T>) {
  const {
    control,
    formState: { errors },
  } = useFormContext<T>();

  const errorMessage = errors[name]?.message as string | undefined;

  return (
    <FormGroup>
      <Controller
        name={name}
        control={control}
        render={({ field }) => (
          <FormControlLabel
            control={
              <MuiCheckbox
                {...field}
                checked={field.value}
                size="small"
                {...rest}
              />
            }
            label={label || ""}
          />
        )}
      />
      {errorMessage && <FormHelperText error>{errorMessage}</FormHelperText>}
    </FormGroup>
  );
}

export default Checkbox;
